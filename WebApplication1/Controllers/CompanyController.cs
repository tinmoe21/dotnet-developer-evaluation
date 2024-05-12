using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private List<Company> Companies = new List<Company>();
        
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            if (Companies.Count == 0)
            {
                await LoadCompanies("https://raw.githubusercontent.com/openpolytechnic/dotnet-developer-evaluation/main/xml-api/1.xml");
                await LoadCompanies("https://raw.githubusercontent.com/openpolytechnic/dotnet-developer-evaluation/main/xml-api/2.xml");
            }

            if (Companies.Count == 0)
                return NotFound();

            return Ok(Companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if(Companies.Count == 0)
            {
                await LoadCompanies("https://raw.githubusercontent.com/openpolytechnic/dotnet-developer-evaluation/main/xml-api/1.xml");
                await LoadCompanies("https://raw.githubusercontent.com/openpolytechnic/dotnet-developer-evaluation/main/xml-api/2.xml");
            }

            //if(Companies.Count == 0)
            //    return NotFound();

            var selected = Companies.FirstOrDefault(x => x.Id == id);
            return Ok(selected);
        }

        private async Task LoadCompanies(string fileName)
        {
            HttpClient client = new HttpClient();
            // Accept Header is set to get response in XML format
            client.DefaultRequestHeaders.Add("Accept", "application/xml");

            string xml = string.Empty;
            //HttpContent body = new StringContent(xml, Encoding.UTF8, "application/xml");
            var response = client.GetAsync(fileName).Result;

            // Check Response Status Code
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var responseserializer = new XmlSerializer(typeof(CompanyXMLModel));
                    using (StringReader reader = new StringReader(content))
                    {
                        CompanyXMLModel data = (CompanyXMLModel)responseserializer.Deserialize(reader);
                        if (data != null)
                        {
                            Company co = new()
                            {
                                Id = data.Id,
                                Description = data.Description,
                                Name = data.Name
                            };
                            Companies.Add(co);
                        }
                    }
                }
            }
        }
    }
}
