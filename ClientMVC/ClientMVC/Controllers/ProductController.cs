using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ClientMVC.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            try
            {
                ViewBag.Message = "Your application description page.";
                WebClient proxy = new WebClient();
                string serviceurl = string.Format("http://localhost:53985/api/products");
                proxy.Headers.Add(string.Format("authorization: bearer {0}", System.Web.HttpContext.Current.Session["WebApi_access_token"]));
                byte[] data = proxy.DownloadData(serviceurl);
                Stream mem = new MemoryStream(data);
                var reader = new StreamReader(mem);
                var result = reader.ReadToEnd();
                var model = JsonConvert.DeserializeObject<List<ClientMVC.Models.Product>>(result);
                return View(model);
            }
            catch (Exception err) {
                ViewBag.Message = "Your application description page.";
                return View();
            }                        
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Message = "List of created products";
            WebClient proxy = new WebClient();
            string serviceurl = string.Format("http://localhost:53985/api/products/{0}", id);
            proxy.Headers.Add(string.Format("authorization: bearer {0}", System.Web.HttpContext.Current.Session["WebApi_access_token"]));
            byte[] data = proxy.DownloadData(serviceurl);
            Stream mem = new MemoryStream(data);
            var reader = new StreamReader(mem);
            var result = reader.ReadToEnd();
            var model = JsonConvert.DeserializeObject<ClientMVC.Models.Product>(result);

            return View(model);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            ClientMVC.Models.Product Product = new Models.Product();
            try
            {
                Product.Name = collection["Name"].ToString();
                Product.ProductNumber = collection["ProductNumber"].ToString();
                Product.Color = collection["Color"].ToString();
                Product.StandardCost = Convert.ToDecimal(collection["StandardCost"].ToString());
                Product.ListPrice = Convert.ToDecimal(collection["ListPrice"].ToString());
                Product.Size = collection["Size"].ToString();
                Product.Weight = Convert.ToDecimal(collection["Weight"].ToString());
                Product.ProductCategoryID = Convert.ToInt32(collection["ProductCategoryID"].ToString());
                Product.ProductModelID = Convert.ToInt32(collection["ProductModelID"].ToString());
                Product.SellStartDate = Convert.ToDateTime(collection["SellStartDate"].ToString());
                Product.SellEndDate = Convert.ToDateTime(collection["SellEndDate"].ToString());
                Product.DiscontinuedDate = Convert.ToDateTime(collection["DiscontinuedDate"].ToString());
                //Product.ThumbNailPhoto = Convert.ToDecimal(collection["ThumbNailPhoto"].ToString());
                //Product.ThumbnailPhotoFileName = collection["ThumbnailPhotoFileName"].ToString();
                Product.rowguid = Guid.NewGuid();
                Product.ModifiedDate = DateTime.Now;

                string jsonObject = JsonConvert.SerializeObject(Product);
                string serviceurl = string.Format("http://localhost:53985/api/products");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceurl);
                httpWebRequest.Headers.Add(string.Format("authorization: bearer {0}", System.Web.HttpContext.Current.Session["WebApi_access_token"]));
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json; charset=utf-8";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonObject);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    var streamReader = new StreamReader(httpResponse.GetResponseStream());
                }

                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                return View(Product);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            //ViewBag.Message = "List of created products";
            WebClient proxy = new WebClient();
            string serviceurl = string.Format("http://localhost:53985/api/products/{0}", id);
            proxy.Headers.Add(string.Format("authorization: bearer {0}", System.Web.HttpContext.Current.Session["WebApi_access_token"]));
            byte[] data = proxy.DownloadData(serviceurl);
            Stream mem = new MemoryStream(data);
            var reader = new StreamReader(mem);
            var result = reader.ReadToEnd();
            var model = JsonConvert.DeserializeObject<ClientMVC.Models.Product>(result);

            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            ClientMVC.Models.Product Product = new Models.Product();
            try
            {                
                Product.ProductID = Convert.ToInt32(collection["ProductID"].ToString());
                Product.Name = collection["Name"].ToString();
                Product.ProductNumber = collection["ProductNumber"].ToString();
                Product.Color = collection["Color"].ToString();
                Product.StandardCost = Convert.ToDecimal(collection["StandardCost"].ToString());
                Product.ListPrice = Convert.ToDecimal(collection["ListPrice"].ToString());
                Product.Size = collection["Size"].ToString();
                Product.Weight = Convert.ToDecimal(collection["Weight"].ToString());
                Product.ProductCategoryID = Convert.ToInt32(collection["ProductCategoryID"].ToString());
                Product.ProductModelID = Convert.ToInt32(collection["ProductModelID"].ToString());
                Product.SellStartDate = Convert.ToDateTime(collection["SellStartDate"].ToString());
                Product.SellEndDate = Convert.ToDateTime(collection["SellEndDate"].ToString());
                Product.DiscontinuedDate = Convert.ToDateTime(collection["DiscontinuedDate"].ToString());
                //Product.ThumbNailPhoto = Convert.ToDecimal(collection["ThumbNailPhoto"].ToString());
                //Product.ThumbnailPhotoFileName = collection["ThumbnailPhotoFileName"].ToString();
                Product.rowguid = Guid.Parse(collection["rowguid"].ToString());
                Product.ModifiedDate = Convert.ToDateTime(collection["ModifiedDate"].ToString());

                string jsonObject = JsonConvert.SerializeObject(Product);
                string serviceurl = string.Format("http://localhost:53985/api/products/{0}", id);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceurl);
                httpWebRequest.Headers.Add(string.Format("authorization: bearer {0}", System.Web.HttpContext.Current.Session["WebApi_access_token"]));
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Accept = "application/json; charset=utf-8";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {                    
                    streamWriter.Write(jsonObject);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    var streamReader = new StreamReader(httpResponse.GetResponseStream());
                }

                //var response = await client.PostAsJsonAsync(serviceurl, seria);

                return RedirectToAction("Index");
            }
            catch(Exception err)
            {
                return View(Product);
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
