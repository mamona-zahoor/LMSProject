﻿using System;
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
        public ActionResult AddAllBooks(All_Books b)
        {
            try
            {
                using (LMSEntities3 db = new LMSEntities3())
                {
                    AllBooks bk = new AllBooks();
                    bk.Name = b.Name;
                    bk.Number = b.Number;
                    bk.Price = b.Price;
                    bk.Author = b.Author;
                    bk.Edition = b.Edition;
                    bk.Status = b.Status;

                    db.All_Books.Add(b);
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


        public int Fine(DateTime t1, DateTime t2)
        {
            /*TimeSpan t = t1.Subtract(t2);
            int  f = (int)t.TotalDays;
            return f * 50;
            */
            int fine;
            if (t1 == null)
            {
                if (DateTime.Today >= t2)
                {
                    TimeSpan r = t2.Subtract(DateTime.Today);
                    fine = (int)r.TotalDays;
                    return fine * 50;
                }
                else
                {

                    return fine = 0;
                }
            }
            else
            {
                TimeSpan t = t1.Subtract(t2);
                fine = (int)t.TotalDays;
                return fine * 50;
            }
        }


        [HttpPost]
        public ActionResult AddIssuedbook(Issued_Books v)
        {
            try
            {
                LMSEntities3 db = new LMSEntities3();
                IssuedBooksVM m = new IssuedBooksVM();
                foreach(All_Books a in db.All_Books)
                {
                    if(a.Number == v.Number)
                    {
                        m.Number = v.Number;
                    }
                }
                if(m.Number == "")
                {
                    ViewBag.Error = "This Book Number does not Exist in our Libraray";
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
                v.Fine = Fine(v.Return_date, v.Due_date);
                m.Fine = v.Fine;
                m.Status = v.Status;


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
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult AddStudent(tbl_student s, User e)
        {
            try
            {
                Student st = new Student();
                st.Email = s.Email;
                e.Email = s.Email;
                e.ID = 2;
                st.Name = s.Name;
                st.Registration_Number = s.Registration_Number;
                LMSEntities3 db = new LMSEntities3();
                db.tbl_student.Add(s);
                db.Users.Add(e);
                db.SaveChanges();
                SendEMail(s.Email,"CSE Library Membership", "Your request for library membership has been accepted.");

                return RedirectToAction("Student");
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
        public ActionResult AddTeacher(tbl_teacher t, User e)
        {
            try
            {
                LMSEntities3 DB = new LMSEntities3();
                Teacher teacher = new Teacher();
                teacher.Name = t.Name;
                teacher.Email = t.Email;
               e.Email = t.Email;
                e.ID = 1;
                teacher.Designation = t.Designation;

               
                DB.tbl_teacher.Add(t);
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
            LMSEntities3 db = new LMSEntities3();
            foreach (User u in db.Users)
            {
                if (db.tbl_student.Find(id).Email == u.Email)
                {
                    db.Users.Remove(u);
                }
            }
            foreach (tbl_student t1 in db.tbl_student)
            {
                if (t1.ID == id)
                {
                    db.tbl_student.Remove(t1);

                }
            }
            db.SaveChanges();
            return RedirectToAction("Student");
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            LMSEntities3 db = new LMSEntities3();
            foreach (User u in db.Users)
            {
                if (db.tbl_teacher.Find(id).Email == u.Email)
                {
                    db.Users.Remove(u);
                }
            }
            foreach (tbl_teacher t1 in db.tbl_teacher)
            {
                if (t1.ID == id)
                {
                    db.tbl_teacher.Remove(t1);
                   
                }
            }
            db.SaveChanges();
            return RedirectToAction("Teacher");
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
            LMSEntities3 db = new LMSEntities3();
            foreach(All_Books a in db.All_Books)
            {
                if(a.ID == id)
                {
                    db.All_Books.Remove(a);
                }
            }
            db.SaveChanges();
            return RedirectToAction("AllBooks");
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
            db.Issued_Books.Find(id).Fine = t.Fine;
            db.Issued_Books.Find(id).Status = t.Status;

            db.SaveChanges();


            return RedirectToAction("IssuedBooks");
        }



        public ActionResult DeleteIssuedBooks(int id)
        {
            LMSEntities3 db = new LMSEntities3();
            foreach (Issued_Books a in db.Issued_Books)
            {
                if (a.ID == id)
                {
                    db.Issued_Books.Remove(a);
                }
            }
            db.SaveChanges();
            return RedirectToAction("IssuedBooks");
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Teacher t)
        {
            try
            {
                return View();
               
            }
            catch
            {
                return View();
            }
        }


      
    }
}
