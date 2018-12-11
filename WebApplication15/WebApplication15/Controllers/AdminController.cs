using System;
using Microsoft.AspNet.Identity;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication15.Models;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace WebApplication15.Controllers
{
    public class AdminController : Controller
    {
        
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        private void SendEMail(string emailid, string subject, string body)
        {
           
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;


                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("cselibmansys@gmail.com", "LibManSys123");
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.From = new MailAddress("cselibmansys@gmail.com");
                msg.To.Add(new MailAddress(emailid));

                msg.Subject = subject;
                msg.IsBodyHtml = true;
                msg.Body = body;

                client.Send(msg);
           
            
        }


        


        public ActionResult AllBooks(string searchby, string search)
        {
            using (LMSEntities3 db = new LMSEntities3())
            {
                if (searchby == "Name")
                {
                    return View(db.All_Books.Where(x => x.Name.Contains(search) || search == null).ToList());
                }
                else
                {
                    return View(db.All_Books.Where(x => x.Number.Contains(search) || search == null).ToList());
                }
            }
        }
        public ActionResult AddAllBooks()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAllBooks(AllBooks b)
        {
            try
            {
                using (LMSEntities3 db = new LMSEntities3())
                {
                    All_Books bk = new All_Books();
                    bk.Name = b.Name;
                    bk.Number = b.Number;
                    bk.Price = b.Price;
                    bk.Author = b.Author;
                    bk.Edition = b.Edition;
                    bk.Status = b.Status;

                    db.All_Books.Add(bk);
                    db.SaveChanges();

                }
              

                return RedirectToAction("AllBooks");
            }
            catch
            {
                return View();
            }
        }



        public ActionResult Student(string searchby, string search)
            {
            using (LMSEntities3 db = new LMSEntities3())
            {
                if (searchby == "Name")
                {
                    return View(db.tbl_student.Where(x => x.Name.Contains(search) || search == null).ToList());
                }
                else
                {
                    return View(db.tbl_student.Where(x => x.Registration_Number.Contains(search) || search == null).ToList());
                }

            }

        }
        public ActionResult AddStudent()
        {
            return View();
        }
        public ActionResult IssuedBooks(string searchby, string search)
        {
            using (LMSEntities3 db = new LMSEntities3())
            {
                if (searchby == "Number")
                {
                    return View(db.Issued_Books.Where(x => x.Number.Contains(search) || search == null).ToList());
                }
                else
                {
                    return View(db.Issued_Books.Where(x => x.Email.Contains(search) || search == null).ToList());
                }


            }

        }



        public ActionResult AddIssuedbook()
        {
            return View();
        }

        /*  foreach(User u in db.Users)
                 {
                     if(u.Email == v.Email)
                     {
                         v.UserID = u.UserID;
                     }
                 }
                 */
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public int Fine( DateTime t2)
        {
           
            
            int fine;
           
                if (DateTime.Today > t2)
                {
                    TimeSpan r = DateTime.Today.Subtract(t2);
                    fine = (int)r.TotalDays;
                    return fine * 50;
                }
                else
                {

                    return fine = 0;
                }
         
        }
        public bool CheckNumber(string n)
        {
            LMSEntities3 db = new LMSEntities3();
            foreach (All_Books a in db.All_Books)
            {
                if (a.Number == n)
                {
                    return true;
                }
            }
            foreach (Issued_Books b in db.Issued_Books)
            {
                if (b.Number == n)
                {
                    return false;
                }
            }
            return false;
        }



        public bool CheckEmail(string n)
        {
            LMSEntities3 db = new LMSEntities3();
            foreach(User u in db.Users)
            {
                if(u.Email == n)
                {
                    return true;
                }
            }
            return false;
        }


        public bool Checkdates(DateTime r, DateTime i, DateTime d)
        {
            if(i  < r)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkstatus(string n)
        {
            LMSEntities3 db = new LMSEntities3();
            if(n!= "")
            {
                
                foreach(All_Books v in db.All_Books)
                {
                    if(v.Number == n)
                    {
                        if(v.Status == "Available")
                        {
                            return true;
                        }
                    }
                }
            }
           
                return false;
            
        }
        public bool checkuid(string n, int ID)
        {
            LMSEntities3 db = new LMSEntities3();
            foreach(User u in db.Users)
            {
                if(u.Email == n)
                {
                    ID = u.UserID;
                    return true;
                }
            }
            return false;
        }




        [HttpPost]
        public ActionResult AddIssuedbook(Issued_Books v)
        {
            try
            {
                /* LMSEntities3 db = new LMSEntities3();
                
                if (CheckNumber(v.Number) == true )
                {
                    m.Number = v.Number;
                    if(CheckEmail(v.Email) == true)
                    {
                        m.Email = v.Email;
                        if (checkuid(m.Email, v.ID) == true)
                        {
                            
                                v.Fine = Fine(v.Due_date);
                                m.Fine = v.Fine;
                                if (v.Fine == 0)
                                {
                                    v.Status = "Paid";
                                    m.UserID = v.UserID;


                                    db.Issued_Books.Add(v);

                                    db.SaveChanges();
                                    string s = "You Just issued a book from CSE Library!!!";
                                    string b = "Recently you issued book (" + m.Number + ")   details are given below         " + "Issue Date" + m.Issue_date + " " + "Due Date" + m.Due_date + "    if you will not return book on duedate fine will be charged Rs 50 per day";
                                    SendEMail(m.Email, s, b);

                                    return RedirectToAction("IssuedBooks");
                                }
                            }
                        
                    }
                  
                    
                }


                ViewBag.Error = "Errors";
                return View();







                */ LMSEntities3 db = new LMSEntities3();
                IssuedBooksVM m = new IssuedBooksVM();

                foreach (All_Books a in db.All_Books)
                  {
                      if(a.Number == v.Number)
                      {
                          m.Number = v.Number;
                          break;
                      }
                  }
                  if(m.Number == "")
                  {
                      ViewBag.Message = "This Book Number does not Exist in our Libraray";
                      return View();
                  }

                  m.Email = v.Email;
                  foreach (User u in db.Users)
                  {
                      if (u.Email == v.Email)
                      {
                          v.UserID = u.UserID;
                      }
                  }

                  m.Issue_date = v.Issue_date;
                  m.Return_date = v.Return_date;
                  m.Due_date = v.Due_date;
                  v.Fine = Fine( v.Due_date);
                  m.Fine = v.Fine;
                  if (v.Fine == 0)
                  {
                      v.Status = "Paid";
                  }
                  else
                  { m.Status = v.Status; }


                  m.UserID = v.UserID;


                  db.Issued_Books.Add(v);

                  db.SaveChanges();
                  string s = "You Just issued a book from CSE Library!!!";
                  string b = "Recently you issued book (" + m.Number + ")   details are given below         " + "Issue Date" + m.Issue_date + " " + "Due Date" + m.Due_date + "    if you will not return book on duedate fine will be charged Rs 50 per day";
                  SendEMail(m.Email, s, b);

                  return RedirectToAction("IssuedBooks");
                  
            }
            catch
            {

                ViewBag.Error = "Errors";
                return View();
               
            }
            
        }
        [HttpPost]
        public ActionResult AddStudent(Student s, User e)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    tbl_student st = new tbl_student();

                    st.Email = s.Email;
                    e.Email = s.Email;
                    e.ID = 2;
                    st.Name = s.Name;
                    st.Registration_Number = s.Registration_Number;
                    LMSEntities3 db = new LMSEntities3();
                    db.tbl_student.Add(st);
                    db.Users.Add(e);
                    db.SaveChanges();
                    SendEMail(s.Email, "CSE Library Membership", "Your request for library membership has been accepted.");

                    return RedirectToAction("Student");
                }
                return ViewBag.Message = "O";
            }
            catch
            {
                return View();
            }

        }
        public ActionResult Teacher(string searchby, string search)
        {
            using (LMSEntities3 db = new LMSEntities3())
            {
                if(searchby == "Name")
                {
                    return View(db.tbl_teacher.Where(x=> x.Name.Contains(search) || search == null).ToList());
                }
                else 
                {
                    return View(db.tbl_teacher.Where(x => x.Designation.Contains(search) || search == null).ToList());
                }
               
              
                
            }
        }
        public ActionResult AddTeacher()
        {
            return View();
        }
        // GET: Admin/Details/5

      /*  public void addemail()
        {
            LibraryManagementSystemEntities db = new LibraryManagementSystemEntities();
            List<tbl_teacher> teachers = db.tbl_teacher.ToList();

            List<Email> lst = teachers.Select(x => new Email { Email1 = x.Email }).ToList();
            int count = lst.Count();

            int i = 1;
            while (i < count)
            {
                db.Emails.Add(lst[i]);
                ++i;
                db.SaveChanges();

            }
        }
        */
        [HttpPost]
        public ActionResult AddTeacher(Teacher t, User e)
        {
            try
            {
                LMSEntities3 DB = new LMSEntities3();
                tbl_teacher teacher = new tbl_teacher();
                teacher.Name = t.Name;
                teacher.Email = t.Email;
               e.Email = t.Email;
                e.ID = 1;
                teacher.Designation = t.Designation;

               
                DB.tbl_teacher.Add(teacher);
               DB.Users.Add(e);
                
                DB.SaveChanges();

                SendEMail(t.Email, "CSE Library Membership", "Your request for library membership has been accepted.");



                return RedirectToAction("Teacher");
            }
            catch
            {
                return View();
            }
           
        }
        public ActionResult Details(int id)
        {
            LMSEntities3 db = new LMSEntities3();
            string i = db.Issued_Books.Find(id).Email;

            foreach (User u in db.Users)
            {
                if (u.Email == i)
                {
                    if (u.ID == 1)
                    {
                        return RedirectToAction("Detailteacher", new { email = u.Email });
                        //Detailteacher(u.Email);
                    }
                    else
                    {
                        return RedirectToAction("Detailst", new { email = u.Email });
                        // Detailst(u.Email);
                    }
                }
            }

            return View();
        }

        public ActionResult Detailteacher(string email)
        {
            LMSEntities3 db = new LMSEntities3();
            foreach (tbl_teacher t in db.tbl_teacher)
            {
                if (t.Email == email)
                {
                    return View(t);
                }
            }
            return View();

        }

        public ActionResult Detailst(string email)
        {
            LMSEntities3 db = new LMSEntities3();
            foreach (tbl_student t in db.tbl_student)
            {
                if (t.Email == email)
                {
                    return View(t);
                }
            }
            return View();

        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            Teacher t = new Teacher();
            LMSEntities3 db = new LMSEntities3();
            foreach(tbl_teacher T in db.tbl_teacher)
            {
                if(T.ID == id)
                {
                    t.Name = T.Name;
                    t.Email = T.Email;
                    t.Designation = T.Designation;
                    break;

                }
            }
           
            return View(t);
        }
        public ActionResult Editst(int id)
        {
            Student st = new Student();
            LMSEntities3 db = new LMSEntities3();
            foreach (tbl_student ST in db.tbl_student)
            {
                if (ST.ID == id)
                {
                    st.Name = ST.Name;
                    st.Email = ST.Email;
                    st.Registration_Number = ST.Registration_Number;
                    break;

                }
            }

            return View(st);
        }
        [HttpPost]
        public ActionResult Editst(int id, Student t)
        {

            LMSEntities3 db = new LMSEntities3();
                db.tbl_student.Find(id).Name = t.Name;
            foreach(User u in db.Users)
            {
                if(db.tbl_student.Find(id).Email == u.Email)
                {
                    u.Email = t.Email;
                }
            }
                db.tbl_student.Find(id).Email = t.Email;

                db.tbl_student.Find(id).Registration_Number = t.Registration_Number;
                db.SaveChanges();


                return RedirectToAction("Student");
            }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Teacher t)
        {
            {
                LMSEntities3 db = new LMSEntities3();
                db.tbl_teacher.Find(id).Name = t.Name;
                foreach (User u in db.Users)
                {
                    if (db.tbl_teacher.Find(id).Email == u.Email)
                    {
                        u.Email = t.Email;
                    }
                }

                db.tbl_teacher.Find(id).Email = t.Email;
                
                db.tbl_teacher.Find(id).Designation = t.Designation;
                db.SaveChanges();


                return RedirectToAction("Teacher");
            }
            /*catch
            {
                return View();
            }*/
        }
        public ActionResult Deletest(int id)
        {
            ViewBag.Title = "Delete Student";
            LMSEntities3 db = new LMSEntities3();
            tbl_student b = db.tbl_student.Find(id);
            return View(b);
        }

        [HttpPost]
        public ActionResult Deletest(int id, Student obj)
        {
            try
            {
                ViewBag.Title = "Delete Student";
                LMSEntities3 db = new LMSEntities3();
                var ToDelete = db.tbl_student.Single(x => x.ID == id);
                //var ToDelete1 = db.Users.Single(x => x.ID == id);
                db.tbl_student.Remove(ToDelete);
               // db.Users.Remove(ToDelete1);
                db.SaveChanges();
                return RedirectToAction("Student");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Title = "Delete Teacher";
            LMSEntities3 db = new LMSEntities3();
            tbl_teacher b = db.tbl_teacher.Find(id);
            return View(b);
        }

        [HttpPost]
        public ActionResult Delete(int id, Teacher obj)
        {
            try
            {
                ViewBag.Title = "Delete Teacher";
                LMSEntities3 db = new LMSEntities3();
                var ToDelete = db.tbl_teacher.Single(x => x.ID == id);
                //var ToDelete1 = db.Users.Single(x => x.ID == id);
                db.tbl_teacher.Remove(ToDelete);
               // db.Users.Remove(ToDelete1);

                db.SaveChanges();
                return RedirectToAction("Teacher");
            }
            catch (Exception)
            {
                return View();
            }
        }




        public ActionResult Editallbooks(int id)
        {
            AllBooks st = new AllBooks();
            LMSEntities3 db = new LMSEntities3();
            foreach (All_Books ST in db.All_Books)
            {
                if (ST.ID == id)
                {
                    st.Name = ST.Name;
                    st.Author = ST.Author;
                    st.Price = ST.Price;
                    st.Number = ST.Number;
                    st.Edition = ST.Edition;
                    st.Status = ST.Status;
                    break;

                }
            }

            return View(st);
        }
        [HttpPost]
        public ActionResult Editallbooks(int id, AllBooks t)
        {

            LMSEntities3 db = new LMSEntities3();
            db.All_Books.Find(id).Name = t.Name;
            db.All_Books.Find(id).Author = t.Author;
            db.All_Books.Find(id).Number = t.Number;
            db.All_Books.Find(id).Price = t.Price;
            db.All_Books.Find(id).Edition = t.Edition;
            db.All_Books.Find(id).Status = t.Status;


            db.SaveChanges();


            return RedirectToAction("AllBooks");
        }



        public ActionResult DeleteAllBooks(int id)
        {
            ViewBag.Title = "Delete Book";
            LMSEntities3 db = new LMSEntities3();
            All_Books b = db.All_Books.Find(id);
            return View(b);
        }

        [HttpPost]
        public ActionResult DeleteAllBooks(int id, AllBooks obj)
        {
            try
            {
                ViewBag.Title = "Delete Book";
                LMSEntities3 db = new LMSEntities3();
                var ToDelete = db.All_Books.Single(x => x.ID == id);
                db.All_Books.Remove(ToDelete);
                db.SaveChanges();
                return RedirectToAction("AllBooks");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult Editissuedbooks(int id)
        {
            IssuedBooksVM st = new IssuedBooksVM();
            LMSEntities3 db = new LMSEntities3();
            foreach (Issued_Books ST in db.Issued_Books)
            {
                if (ST.ID == id)
                {
                    st.Number = ST.Number;
                    st.Email = ST.Email;
                    st.Issue_date = ST.Issue_date;
                    st.Return_date = ST.Return_date;
                    st.Due_date = ST.Due_date;
                    st.Fine = ST.Fine;
                    st.Status = ST.Status;
                    break;

                }
            }

            return View(st);
        }
        [HttpPost]
        public ActionResult Editissuedbooks(int id, IssuedBooksVM t)
        {

            LMSEntities3 db = new LMSEntities3();
            db.Issued_Books.Find(id).Number = t.Number;
            db.Issued_Books.Find(id).Email = t.Email;
            db.Issued_Books.Find(id).Issue_date = t.Issue_date;
            db.Issued_Books.Find(id).Return_date = t.Return_date;
            db.Issued_Books.Find(id).Due_date = t.Due_date;
            db.Issued_Books.Find(id).Fine = Fine( t.Due_date);
            if (db.Issued_Books.Find(id).Fine == 0)
            {
                db.Issued_Books.Find(id).Status = "Paid";
            }
            else
            { db.Issued_Books.Find(id).Status = t.Status; }

            db.SaveChanges();


            return RedirectToAction("IssuedBooks");
        }



        // Admin/Delete/Issued Books
        public ActionResult DeleteIssuedBooks(int id)
        {
            ViewBag.Title = "Delete Book";
            LMSEntities3 db = new LMSEntities3();
            Issued_Books b = db.Issued_Books.Find(id);
            return View(b);
        }

        [HttpPost]
        public ActionResult DeleteIssuedBooks(int id, IssuedBooksVM obj)
        {
            try
            {
                ViewBag.Title = "Delete Book";
                LMSEntities3 db = new LMSEntities3();
                var ToDelete = db.Issued_Books.Single(x => x.ID == id);
                db.Issued_Books.Remove(ToDelete);
                db.SaveChanges();
                return RedirectToAction("IssuedBooks");
            }
            catch (Exception)
            {
                return View();
            }
        }


      
    }
}
