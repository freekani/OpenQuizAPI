using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using OpenQuizAPI.Models;
using OpenQuizAPI.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;



namespace Step.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Quiz4Controller : ControllerBase
    {

        [HttpGet("[action]")]
        public ActionResult<CustomPair> getCustomPairFormat()
        {
            return new CustomPair() { type = "type", var = "var" };
        }


        // GET: api/Quiz4
        [HttpGet]
        public ActionResult<List<int>> GetQuizList()
        {
            return pullIntList("SELECT QuizId FROM Quiz4;");
        }

        [Route("[action]")]
        [HttpPost]

        public ActionResult<Quiz4> PostQuizforQuiz4(Quiz4 newquiz)
        {
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            IEnumerable<int> result;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                result = connection.Query<int>("insert into Quiz4(Question,A0,A1,A2,A3,Answer,Genre,Presenter) values(@Question,@A0,@A1,@A2,@A3,@Answer,@Genre,@Presenter);SELECT last_insert_id();", new
                {
                    Question = newquiz.Question,
                    A0 = newquiz.A0,
                    A1 = newquiz.A1,
                    A2 = newquiz.A2,
                    A3 = newquiz.A3,
                    Answer = newquiz.Answer,
                    Genre = newquiz.Genre,
                    Presenter = newquiz.Presenter
                });
                connection.Close();
            }
            newquiz.QuizId = result.First();
            return Created(this.GetQuiz(newquiz.QuizId).ToString(), newquiz);
        }
        [Route("")]
        [HttpPost]
        public ActionResult<Quiz4> PostQuizforQuiz4Post(Quiz4Post newquiz)
        {
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            IEnumerable<int> newid;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                newid = connection.Query<int>("insert into Quiz4(Question,A0,A1,A2,A3,Answer,Genre,Presenter) values(@Question,@A0,@A1,@A2,@A3,@Answer,@Genre,@Presenter);SELECT last_insert_id();", new
                {
                    Question = newquiz.Question,
                    A0 = newquiz.A0,
                    A1 = newquiz.A1,
                    A2 = newquiz.A2,
                    A3 = newquiz.A3,
                    Answer = newquiz.Answer,
                    Genre = newquiz.Genre,
                    Presenter = newquiz.Presenter
                });
                connection.Close();
            }
            Quiz4 result = new Quiz4()
            {
                QuizId = newid.First(),
                Question = newquiz.Question,
                A0 = newquiz.A0,
                A1 = newquiz.A1,
                A2 = newquiz.A2,
                A3 = newquiz.A3,
                Answer = newquiz.Answer,
                Genre = newquiz.Genre,
                Presenter = newquiz.Presenter
            };

            return Created(this.GetQuiz(result.QuizId).ToString(), result);
        }


        [HttpGet("{QuizId}")]
        public ActionResult<Quiz4> GetQuiz(long QuizId)
        {
            return this.pullQuiz(QuizId);
        }

        [HttpGet("{QuizId}/{Answer}")]
        public ActionResult<bool> Check(long QuizId, int Answer)
        {
            var checking = this.pullQuiz(QuizId);
            if (checking == null)
            {
                return NotFound();
            }
            return checking.Answer == Answer;
        }





        [HttpGet("Genre")]
        public ActionResult<List<String>> getGenreList()
        {
            return pullStringList("SELECT DISTINCT Genre FROM Quiz4;");
        }

        [HttpGet("Genre/{Genre}")]
        public ActionResult<List<int>> searchGenre(String Genre)
        {
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            List<int> result;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                result = connection.Query<int>("SELECT QuizId FROM Quiz4 WHERE Genre = @Genre;", new
                {
                    Genre = Genre
                }).ToList();
                connection.Close();
            }
            return result;
        }



        [HttpGet("Presenter")]
        public ActionResult<List<String>> getPresenterList()
        {
            return pullStringList("SELECT DISTINCT Presenter FROM Quiz4;");
        }

        [HttpGet("Presenter/{Presenter}")]//ここ本当はidまでをget、Answerをpostでとりたかった
        public ActionResult<List<int>> searchPresenter(String Presenter)
        {
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            List<int> result;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                result = connection.Query<int>("SELECT QuizId FROM Quiz4 WHERE Presenter = @Presenter;", new
                {
                    Presenter = Presenter
                }).ToList();
                connection.Close();
            }
            return result;
        }
        [HttpPost("CustomList")]
        public ActionResult<List<Quiz4>> getCustomList(CustomPair postData)//ここでこう値をとれないことを知りました！
        {
            if (postData == null)
            {
                return BadRequest();
            }
            var result = pullCustomList(postData.type, postData.var);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
        [HttpPost]
        [HttpPost("CustomOne")]
        public ActionResult<Quiz4> getCustomOne(CustomPair postData)
        {
            var result = pullCustomList(postData.type, postData.var);
            if (result == null)
            {
                return NotFound();
            }
            return result[new Random().Next(result.Count())];
        }

        [Route("")]

        private ActionResult<List<String>> pullStringList(String Query)
        {//ここに入れるSQL文は、必ずselect string型の何か
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            List<String> result = new List<String>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                result = connection.Query<String>(Query).ToList();
                connection.Close();
            }
            return result;
        }
        private ActionResult<List<Quiz4>> pullQuizList(String Query)
        {//ここに入れるSQL文は、必ずselect *
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            List<Quiz4> result;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                result = connection.Query<Quiz4>(Query).ToList();
                connection.Close();
            }
            return result;
        }
        private ActionResult<List<int>> pullIntList(String Query)
        {//ここに入れるSQL文は、必ずselect int型の何か
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            List<int> result;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                result = connection.Query<int>(Query).ToList();
                connection.Close();
            }
            return result;
        }
        private Quiz4 pullQuiz(long QuizId)
        {//idからクイズを一意に抽出します。（DB依存）
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            List<Quiz4> quiz;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                quiz = connection.Query<Quiz4>("SELECT * FROM Quiz4 WHERE QuizId = @QuizId ;", new
                {
                    QuizId = QuizId
                }).ToList();
                connection.Close();
            }
            if (quiz.Count() == 0)
            {
                return null;
            }

            return quiz.First();
        }

        private List<Quiz4> pullCustomList(String type, String var)
        {
            List<Quiz4> result;
            var ConnectionString = AppConfiguration.Current.ConnectionString;
            using (var connection = new MySqlConnection(ConnectionString))
            {

                var changedType = new Regex("[^(0-9a-zA-Z)]");
                type = changedType.Replace(type, "");
                connection.Open();
                try
                {
                    result = connection.Query<Quiz4>("SELECT * FROM Quiz4 WHERE " + type + " = @var ;", new
                    {
                        var = var
                    }).ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    return null;
                }
                connection.Close();
            }
            if (result.Count() == 0)
            {
                return null;
            }
            return result;
        }



    }

}