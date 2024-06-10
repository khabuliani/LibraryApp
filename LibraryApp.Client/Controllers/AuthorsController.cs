using LibraryApp.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LibraryApp.Client.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IConfiguration _appConfiguration;

        public AuthorsController(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        [HttpGet]
        public async Task<IActionResult> AllAuthors(string name)
        {
            var result = new List<GetAllAuthorDto>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGetAll"];
                    string json = "";
                    var input = new GetAllAuthorInputDto();
                    if (name != null)
                    {
                        input.Name = name;
                    }
                    json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<List<GetAllAuthorDto>>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> AuthorDetails(int id)
        {
            var author = new AuthorDto();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGet"] + id;
                    using (var responseMessage = await client.GetAsync($"{url}"))
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                        author = JsonConvert.DeserializeObject<AuthorDto>(response);
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = new AuthorDto();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGet"] + id;
                    using (var responseMessage = await client.GetAsync($"{url}"))
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                        author = JsonConvert.DeserializeObject<AuthorDto>(response);
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorDto input)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorUpdate"];
                    string json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PutAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            input = JsonConvert.DeserializeObject<AuthorDto>(response);

                        }
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(AllAuthors));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var author = new AuthorDto();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGet"] + id;
                    using (var responseMessage = await client.GetAsync($"{url}"))
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                        author = JsonConvert.DeserializeObject<AuthorDto>(response);
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorDelete"] + id;
                    using (var responseMessage = await client.DeleteAsync($"{url}"))
                    {
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(AllAuthors));
        }

        [HttpGet]   
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorDto input)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorCreate"];
                    string json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            input = JsonConvert.DeserializeObject<AuthorDto>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(AllAuthors));
        }
    }
}
