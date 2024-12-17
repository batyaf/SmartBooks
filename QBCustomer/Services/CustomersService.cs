using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QBCustomer.Models;
using System.Text.Json;


namespace QBCustomer.Services
{
    public class CustomersService
    {

        private SmartBooksContext _db;
        private readonly SbUsersService _sbUsersService;
        private readonly QuickBooksService _quickBooksService;

        public CustomersService(SmartBooksContext db, SbUsersService sbUsersService,QuickBooksService quickBooksService)
        {
            _db = db;
            _sbUsersService = sbUsersService;
            _quickBooksService = quickBooksService;
        }

        //get customer from QuickBooks
        public async Task<object> getCustomerFromQBApi(string userId)
        {
            try
            {
                var customerToken= await GetTokenFromDb(userId);
                if (customerToken == null) { 

                }
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(customerToken.Token);
                ServiceContext serviceContext = new ServiceContext(customerToken.RealmId, IntuitServicesType.QBO, oauthValidator);
                serviceContext.IppConfiguration.MinorVersion.Qbo = "62";
                serviceContext.IppConfiguration.BaseUrl.Qbo = "https://sandbox-quickbooks.api.intuit.com/"; //This is sandbox Url.

                QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);
                Customer? customer = querySvc.ExecuteIdsQuery("SELECT * FROM Customer").FirstOrDefault();

                if (customer != null)
                {
                    string CustomerJson = JsonConvert.SerializeObject(customer, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    CustomerModel customerModel = ConvertJsonToCustomer(CustomerJson);
                    customerModel.User =await _sbUsersService.GetUser(userId);
                    var existingCustomer= await GetCustomerByUserId(userId);
                    CustomerModel qbcustomer;
                    if (existingCustomer != null)
                    {
                        qbcustomer = await UpdateCustomerInDb(existingCustomer, customerModel);
                    }
                    else
                    {
                       qbcustomer= await AddCustomerToDb(customerModel);
                    }
                    return CreateCustomerResponse(qbcustomer, true);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("An error occurred while fetching customer data from QuickBooks.");
            }

        }

  
        public async Task<CustomerModel> AddCustomerToDb(CustomerModel customer)
        {
            try
            {
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                return customer;
            }
            catch (DbUpdateException ex)
            {
                // Log the full exception details
                Console.WriteLine($"Database update error: {ex.Message}");

                // If there's an inner exception, log that too
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                throw; // Re-throw to allow caller to handle or log the error
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error saving customer: {ex.Message}");
                throw;
            }
        }

        public async Task<CustomerModel> UpdateCustomerInDb(CustomerModel existingCustomer, CustomerModel customer)
        {
           
            // Update the scalar properties of the existing customer
            existingCustomer.GivenName = customer.GivenName ?? existingCustomer.GivenName;
            existingCustomer.FamilyName = customer.FamilyName ?? existingCustomer.FamilyName;
            existingCustomer.DisplayName = customer.DisplayName ?? existingCustomer.DisplayName;
            existingCustomer.CompanyName = customer.CompanyName ?? existingCustomer.CompanyName;
            existingCustomer.PrimaryEmailAddr = customer.PrimaryEmailAddr ?? existingCustomer.PrimaryEmailAddr;
            existingCustomer.Active = customer.Active;

            // Update the nested BillAddr property if provided
            if (customer.BillAddr != null)
            {
                existingCustomer.BillAddr.City = customer.BillAddr.City ?? existingCustomer.BillAddr.City;
                existingCustomer.BillAddr.Line1 = customer.BillAddr.Line1 ?? existingCustomer.BillAddr.Line1;
                existingCustomer.BillAddr.PostalCode = customer.BillAddr.PostalCode ?? existingCustomer.BillAddr.PostalCode;
                existingCustomer.BillAddr.Lat = customer.BillAddr.Lat ?? existingCustomer.BillAddr.Lat;
                existingCustomer.BillAddr.Long = customer.BillAddr.Long ?? existingCustomer.BillAddr.Long;
                existingCustomer.BillAddr.CountrySubDivisionCode = customer.BillAddr.CountrySubDivisionCode ?? existingCustomer.BillAddr.CountrySubDivisionCode;
            }

            // Update the PrimaryPhone property if provided
            if (customer.PrimaryPhone != null)
            {
                existingCustomer.PrimaryPhone.FreeFormNumber = customer.PrimaryPhone.FreeFormNumber ?? existingCustomer.PrimaryPhone.FreeFormNumber;
            }

            // Save the changes to the database
            _db.SaveChanges();

            return existingCustomer;
        }

        public async Task<object> GetCustomer(string userId)
        {
            var customer= await GetCustomerByUserId(userId);
            if(customer != null)
            {
              return CreateCustomerResponse(customer,true);
            }
            var token = await GetTokenFromDb(userId);
            return CreateCustomerResponse(customer, token!=null);

        }
        public async Task<CustomerModel?> GetCustomerByUserId(string userId)
        {
            var existingCustomer = await _db.Customers
                .Include(c => c.BillAddr)
                .Include(c => c.PrimaryPhone)
                .Include(c => c.PrimaryEmailAddr)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.User.UserId == userId); 
            return existingCustomer;
        }



        public async Task<CustomerToken?> GetTokenFromDb(string userId)
        {
            var customerToken = await _db.CustomerTokens
           .Include(c => c.User) 
           .FirstOrDefaultAsync(c => c.User.UserId == userId);

            if (customerToken == null)
            {
                return null;
            }

            if (DateTime.UtcNow > customerToken.CreationTokenExpiresAt)
            {
                await _quickBooksService.RefreshAccessTokenAsync(customerToken);
            }
            return customerToken;
        }

       


        public static CustomerModel ConvertJsonToCustomer(string json)
        {
            try
            {
                var customer = JsonConvert.DeserializeObject<CustomerModel>(json, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore 
                });
                return customer;
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return null;
            }
        }


        public string ConvertCustomerToJson(CustomerModel customer)
        {
            try
            {
                string json = JsonConvert.SerializeObject(customer, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, 
                    Formatting = Formatting.Indented 
                });
                return json;
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine($"Error serializing customer: {ex.Message}");
                return null;
            }
        }

        public object CreateCustomerResponse(CustomerModel customer,bool isAuthorticated)
        {
            return new
            {
                IsAuthorticated = isAuthorticated,
                Customer = customer!= null? new
                {
                    BasicInfo = new { customer.FullyQualifiedName, customer.DisplayName, customer.CompanyName, customer.Active },
                    ContactInfo = new { customer.PrimaryEmailAddr?.Address, customer.PrimaryPhone?.FreeFormNumber },
                    BillingInfo = new { customer.Balance, customer.BalanceWithJobs, customer.Taxable, customer.PreferredDeliveryMethod },
                    AdditionalInfo = new { customer.Job, customer.BillWithParent, customer.QuickBooksId }
                }: null,
            };
        }


     

    }
}
