using LibraryApp.Client.Models;
using LibraryApp.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Text;

namespace LibraryApp.Client.Controllers
{
    public class BooksController : Controller
    {
        private readonly IConfiguration _appConfiguration;
        public BooksController(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        [HttpGet]
        public async Task<IActionResult> AllBooks(string title)
        {
            var result = new List<GetAllBookDto>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:BookGetAll"];
                    var input = new GetAllBookInputDto() { Title = title };
                    string json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<List<GetAllBookDto>>(response);
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
        public async Task<IActionResult> BookDetails(int id)
        {
            var book = new BookDto();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:BookGet"] + id;
                    using (var responseMessage = await client.GetAsync($"{url}"))
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<BookDto>(response);
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = new BookDto();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:BookGet"] + id;
                    using (var responseMessage = await client.GetAsync($"{url}"))
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<BookDto>(response);
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            if (book == null)
            {
                return NotFound();
            }
            var authors = new List<GetAllAuthorDto>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGetAll"];
                    string json = "";
                    var input = new GetAllAuthorInputDto();
                    json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            authors = JsonConvert.DeserializeObject<List<GetAllAuthorDto>>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            var viewModel = new BookViewModel
            {
                Book = book,
                Authors = authors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName + " " + a.LastName }).ToList(),
                SelectedAuthorIds = book.Connections.Select(a => a.AuthorId).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel viewModel)
        {
            try
            {
                var book = viewModel.Book;
                var authorIds = new List<AuthorConnectionDto>();
                foreach(var authorId in viewModel.SelectedAuthorIds)
                {
                    var author = new AuthorConnectionDto()
                    {
                        AuthorId = authorId,
                        BookId = book.Id
                    };
                    authorIds.Add(author);
                }
                book.Connections = authorIds;
                using (HttpClient client = new HttpClient())
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:BookUpdate"];
                    string json = JsonConvert.SerializeObject(book);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PutAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                        }
                    } 
                }
                return RedirectToAction(nameof(AllBooks));
            }
            catch
            {

            }
            var authors = new List<GetAllAuthorDto>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGetAll"];
                    string json = "";
                    var input = new GetAllAuthorInputDto();
                    json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            authors = JsonConvert.DeserializeObject<List<GetAllAuthorDto>>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            viewModel.Authors = authors
                                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName + " " + a.LastName }).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var author = new BookDto();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:BookGet"] + id;
                    using (var responseMessage = await client.GetAsync($"{url}"))
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                        author = JsonConvert.DeserializeObject<BookDto>(response);
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

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:BookDelete"] + id;
                    using (var responseMessage = await client.DeleteAsync($"{url}"))
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(AllBooks));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var authors = new List<GetAllAuthorDto>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGetAll"];
                    string json = "";
                    var input = new GetAllAuthorInputDto();
                    json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            authors = JsonConvert.DeserializeObject<List<GetAllAuthorDto>>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            var viewModel = new BookViewModel
            {
                Book = new BookDto { IsTaken = false , CreationDate = DateTime.Now},
                Authors = authors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName + " " + a.LastName }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel viewModel)
        {
            try
            {
                var book = viewModel.Book;
                var authorIds = new List<AuthorConnectionDto>();
                StringBuilder a = new StringBuilder();
                a.Append("{ \"Connections\" :[");  
                if (viewModel.SelectedAuthorIds.Count > 0)
                {
                    int count = 0;
                    foreach (var authorId in viewModel.SelectedAuthorIds)
                    {
                        count++;
                        a.Append("{\"AuthorId\":" + authorId + ",\"BookId\":" + book.Id +@"}");
                        if(count < viewModel.SelectedAuthorIds.Count)
                        {
                            a.Append(",");
                        }
                    }
                }
                a.Append("],");
                book.Connections = authorIds;
                using (HttpClient client = new HttpClient())
                {
                    a.Append("\"Picture\":\"" + book.Picture + "\",");
                     a.Append("\"Title\":\"" + book.Title + "\",");
                    a.Append("\"Description\":\"" + book.Description + "\",");
                    a.Append("\"Rate\":" + book.Rate.ToString() + ",");
                    a.Append("\"CreationDate\":\"" + book.CreationDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\",");
                    if (book.IsTaken)
                    {
                        a.Append("\"IsTaken\": true}");
                    }
                    else
                    {
                        a.Append("\"IsTaken\": false}");
                    }
                    var b = a.ToString();
                    //var content = new FormUrlEncodedContent(values);
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:BookCreate"];
                    using (StringContent content = new StringContent(b, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                        }
                    }
                }
                return RedirectToAction(nameof(AllBooks));
            }catch (Exception ex)
            {

            }
            var authors = new List<GetAllAuthorDto>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _appConfiguration["App:ServerRootAddress"] + _appConfiguration["ApiUrls:AuthorGetAll"];
                    string json = "";
                    var input = new GetAllAuthorInputDto();
                    json = JsonConvert.SerializeObject(input);
                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        using (var responseMessage = await client.PostAsync(url, content))
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();
                            authors = JsonConvert.DeserializeObject<List<GetAllAuthorDto>>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            viewModel.Authors = authors
                                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName + " " + a.LastName }).ToList();
            return View(viewModel);
        }
    }
}
