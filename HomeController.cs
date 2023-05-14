using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetFinal.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ProjetFinal.Controllers
{
    public class HomeController : Controller
    {
        public readonly QuizExamenContext context;
        
        public HomeController(QuizExamenContext _context)
        {
            context = _context;
        }

        
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult newquiz()
        {
            Quiz quiz = new Quiz(); // créer une instance de Quiz
            return View(quiz);
        }

       public IActionResult AjoutQuiz(Quiz quiz)
        {
            int nbQuestionsFaciles = int.Parse(Request.Form["nbQuestionsFaciles"]);
            int nbQuestionsMoyennes = int.Parse(Request.Form["nbQuestionsMoyennes"]);
            int nbQuestionsDifficiles = int.Parse(Request.Form["nbQuestionsDifficiles"]);

            var random = new Random();

            var questionsFaciles = context.Question
                .Where(q => q.CategoryId == 1)
                .ToList();
            var questionsFacilesAleatoires = questionsFaciles.OrderBy(q => random.Next()).Take(nbQuestionsFaciles).ToList();

            var questionsMoyennes = context.Question
                .Where(q => q.CategoryId == 2)
                .ToList();
            var questionsMoyennesAleatoires = questionsMoyennes.OrderBy(q => random.Next()).Take(nbQuestionsMoyennes).ToList();

            var questionsDifficiles = context.Question
                .Where(q => q.CategoryId == 3)
                .ToList();
            var questionsDifficilesAleatoires = questionsDifficiles.OrderBy(q => random.Next()).Take(nbQuestionsDifficiles).ToList();

            // Ajouter les questions de type Facile
            foreach (var question in questionsFacilesAleatoires)
            {
                quiz.QuestionQuiz.Add(new QuestionQuiz { QuestionId = question.QuestionId });
            }

            // Ajouter les questions de type Moyen
            foreach (var question in questionsMoyennesAleatoires)
            {
                quiz.QuestionQuiz.Add(new QuestionQuiz { QuestionId = question.QuestionId });
            }

            // Ajouter les questions de type Difficile
            foreach (var question in questionsDifficilesAleatoires)
            {
                quiz.QuestionQuiz.Add(new QuestionQuiz { QuestionId = question.QuestionId });
            }
                                 
            context.Quiz.Add(quiz); // ajouter le quiz à la base de données
            context.SaveChanges(); // Sauvegarder l'objet Quiz dans la base de données

            return RedirectToAction("Index");
        }


        public IActionResult passQuiz()
        {
            return View();
        }

        public IActionResult ListQuiz(string UserName, string Email)
        {
            var quizes = context.Quiz.Where(x => x.UserName == UserName && x.Email == x.Email).ToList();
            return View(quizes);
        }

        //return questions
        public IActionResult answerQuiz(int QuizId)
        {
            //var quiz = new Quiz();
            var questions = context.QuestionQuiz.Where(x => x.QuizId == QuizId).ToList();
            var itemOptions = context.ItemOption.Include(i => i.QuestionId);
            ViewBag.No = QuizId;
            
           
            return View(questions);
        }
        


    }
}
