using AzureTableStorageDemo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;

namespace Employee_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration config;
        public EmployeeController(IConfiguration configuration)
        {
            config = configuration;
        }


        [HttpGet]
        public IEnumerable<EmployeeEntity> Get()
        {
            var query = new TableQuery<EmployeeEntity>();
            // Method 2
            string _dbCon = config.GetValue<string>("ConnectionStrings:MyAzureTable");
            var account = CloudStorageAccount.Parse(_dbCon);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("Employee");
            var lst = table.ExecuteQuery(query);
            return lst.ToList();
        }
        [HttpPost]
        public IEnumerable<EmployeeEntity> Post([FromBody] EmployeeEntity emp)
        {
           
            string _dbCon = config.GetValue<string>("ConnectionStrings:MyAzureTable");
            var account = CloudStorageAccount.Parse(_dbCon);
            var client = account.CreateCloudTableClient();

            var table = client.GetTableReference("Employee");

            table.CreateIfNotExists();

            EmployeeEntity employeeEntity = new EmployeeEntity(emp.FirstName, emp.LastName);
            employeeEntity.FirstName = emp.FirstName;
            employeeEntity.LastName = emp.LastName;
            employeeEntity.PhoneNumber = emp.PhoneNumber;
            employeeEntity.Email = emp.Email;
            var query = new TableQuery<EmployeeEntity>();
            TableOperation insertOperation = TableOperation.Insert(employeeEntity);


            table.Execute(insertOperation);
            var lst = table.ExecuteQuery(query);
            return lst.ToList();

        }

        [HttpPut]
        public IEnumerable<EmployeeEntity> Put([FromBody] EmployeeEntity emp)
        {
           
            string _dbCon = config.GetValue<string>("ConnectionStrings:MyAzureTable");
            var account = CloudStorageAccount.Parse(_dbCon);
            var client = account.CreateCloudTableClient();

            var table = client.GetTableReference("Employee");

            table.CreateIfNotExists();

            EmployeeEntity employeeEntity = new EmployeeEntity(emp.FirstName, emp.LastName);
            employeeEntity.FirstName = emp.FirstName;
            employeeEntity.LastName = emp.LastName;
            employeeEntity.PhoneNumber = emp.PhoneNumber;
            employeeEntity.Email = emp.Email;
            var query = new TableQuery<EmployeeEntity>();
            TableOperation insertOperation = TableOperation.InsertOrMerge(employeeEntity);
            table.Execute(insertOperation);
            var lst = table.ExecuteQuery(query);
            return lst.ToList();

        }

        [HttpDelete]
        public IEnumerable<EmployeeEntity> Delete([FromBody] EmployeeEntity emp)
        {
            
            string _dbCon = config.GetValue<string>("ConnectionStrings:MyAzureTable");
            var account = CloudStorageAccount.Parse(_dbCon);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("Employee");
            table.CreateIfNotExists();
            EmployeeEntity employeeEntity = new EmployeeEntity(emp.FirstName, emp.LastName);
            employeeEntity.FirstName = emp.FirstName;
            employeeEntity.LastName = emp.LastName;
            employeeEntity.PhoneNumber = emp.PhoneNumber;
            employeeEntity.Email = emp.Email;
            employeeEntity.ETag = "*"; // wildcard 
            var query = new TableQuery<EmployeeEntity>();
            TableOperation insertOperation = TableOperation.Delete(employeeEntity);
            table.Execute(insertOperation);
            var lst = table.ExecuteQuery(query);
            return lst.ToList();

        }

    }
}
