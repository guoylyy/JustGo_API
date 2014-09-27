using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Archive.Datas;
using Archive.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Archive
{
    public static class ServerApi
    {
        #region Field

        private const string ServerUrl = "http://tjcsdc.com/V1/";
        private const string Category = "goal_category";//获取goal分类
        private const string Goal = "goal/";//获取goal,需加上goal分类使用
        private const string GoalJoinRecord = "goal_join_record/";//获取某个用户特定任务的record，需加上goalJoinRecord的id使用
        private const string GoalDetail = "goal_details/";//获取某个goal的具体信息，需加上goalId使用
        private const string GoalRecordAwesome = "goal_record_awesome/";//为record点赞或者获取当前点赞内容，需加上Record的id使用
        private const string GoalRecordComment = "goal_record_comment/";//为record评论或者获取当前评论内容，需加上Record的id使用
        private const string Explore = "explore";
        private const string User = "user";   //获取其他用户资料,需加上其他用户的id使用;更新当前用户的资料
        private const string Profile = "user/profile";  //获取当前登录用户个人资料
        private const string Followers = "user/fans";  //获取当前登录用户的粉丝
        private const string Followings = "user/followings";  //获取当前登录用户的关注
        private const string Follow = "user/follow/";  //关注某个用户,需加上关注用户的id使用
        private const string UnFollow = "user/unfollow/"; //取消对某个用户的关注，需加上取消关注用户的id使用
        private const string Search = "user/search/";//搜索用户，需加上搜索字符使用
        private const string Notification = "notification";//获取当前登录用户的通知
        private const string Encourage = "encourage/";//鼓励某个用户，需加上用户id使用
        private const string GoalRecord = "goal_record";//发布新的record
        private const string FightingCenter = "goal_record/fighting_center";//获取用户所有的record
        private const string Login = "login";//用户登录
        private const string GoalRecordList = "goal_record_list/";

        private const string OtherFightingCenter = "goal_record/{0}/fighting_center";
        private const string OtherFollower = "user/{0}/fans";
        private const string OtherFollowing = "user/{0}/followings";

        private const string Post = "POST";
        private const string Get = "GET";
        private const string PostContentType = "application/x-www-form-urlencoded";
        #endregion

        private static ManualResetEvent _waitSignal = new ManualResetEvent(false);

        #region 请求返回码

        public const string CorrectCode = "200";
        public const string BadRequestCode = "400";
        public const string WrongAuthoriedCode = "401";
        public const string ForbiddenCode = "403";
        public const string NotFoundCode = "404";
        public const string PrametersMissingCode = "414";
        public const string InternalErrorCode = "500";

        public const string LoginFailCode = "001";

        public const string EmailExistCode = "011";
        public const string PassWordSimpleCode = "012";

        #endregion

        #region GET方法

        public static async Task GetCategoriesAsync(ObservableCollection<string> goalCategories)
        {
            var url = ServerUrl + Category;
            var result = await GetBaseAsync(url);

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);
            foreach (var category in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(category.ToString());
                string str = d2.category_name;
                goalCategories.Add(str);
            }
        }

        public static async Task GetCategoryGoalsAsync(ObservableCollection<Goal> goals, string category)
        {
            var url = ServerUrl + Goal + category;
            var result = await GetBaseAsync(url);

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);
            foreach (var goal in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(goal.ToString());
                string goalId = d2.goal_id;
                string goalName = d2.goal_name;
                string image = d2.image;
                string description = d2.description;
                int joins = d2.joins;

                var addGoal = new Goal
                {
                    GoalCategory = category,
                    GoalId = goalId,
                    GoalName = goalName,
                    Image = image,
                    Description = description,
                    Participants = joins
                };
                goals.Add(addGoal);
            }
        }

        public static async Task GetExploreAsync(ObservableCollection<Goal> goals)
        {
            var url = ServerUrl + Explore;
            var result = await GetBaseAsync(url);

            dynamic d = JsonConvert.DeserializeObject(result);
            if (d == null) return;

            var list = new List<object>(d);
            foreach (var explore in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(explore.ToString());
                var goal = new Goal
                {
                    GoalId = d2.goal_id,
                    GoalName = d2.goal_name,
                    Image = d2.image,
                    Participants = int.Parse(d2.joins.ToString()),
                    Description = d2.description,
                };
                Deployment.Current.Dispatcher.BeginInvoke(() => goals.Add(goal));

            }
        }

        public static async Task GetUserProfileAsync(User user)
        {
            var url = ServerUrl + Profile;
            var header = new Dictionary<string, string>
            {
                {"Authorization", user.Token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                user.FollowerCount = int.Parse(d.fans.ToString());
                user.FollowingCount = int.Parse(d.followings.ToString());
                user.ImageSource = d.header_icon;
                user.ImageSourceMedium = d.header_icon_medium;
                user.ImageSourceSmall = d.header_icon_small;
                user.UserName = d.name;
                user.UserId = d.user_id;
                user.Description = d.description;
            });
        }

        public static async Task GetUserFollowersAsync(string token, ObservableCollection<User> followers)
        {
            var url = ServerUrl + Followers;
            var header = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());
                var follower = new User
                {
                    UserId = d2.user_id,
                    UserName = d2.user_name,
                    ImageSource = d2.header
                };
                followers.Add(follower);
            }
        }

        public static async Task GetUserFollowingsAsync(string token, ObservableCollection<User> followings)
        {
            var url = ServerUrl + Followings;
            var header = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());
                var follower = new User
                {
                    UserId = d2.user_id,
                    UserName = d2.user_name,
                    ImageSource = d2.header
                };
                followings.Add(follower);
            }

        }

        public static async Task GetOtherUserAsync(string token, User user)
        {
            var url = ServerUrl + User + "/" + user.UserId;
            var header = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);

            user.FollowerCount = int.Parse(d.fans.ToString());
            user.FollowingCount = int.Parse(d.followings.ToString());
            user.ImageSource = d.header_icon;
            user.ImageSourceMedium = d.header_icon_medium;
            user.ImageSourceSmall = d.header_icon_small;
            user.UserName = d.name;
            user.UserId = d.user_id;
            user.Description = d.description;
            user.CanFollow = d.can_follow;
        }

        public static async Task GetSearchUserAsync(string token, string key, ObservableCollection<User> users)
        {
            var url = ServerUrl + Search + key;
            var header = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());
                var user = new User
                {
                    UserId = d2.user_id,
                    UserName = d2.user_name,
                    ImageSource = d2.header
                };
                users.Add(user);
            }
        }

        public static async Task GetNotificationAsync(string token, ObservableCollection<Notification> notifications)
        {
            var url = ServerUrl + Notification;
            var header = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);

            var list = new List<object>(d);
            if (!list.Any()) return;

            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());
                var notification = new Notification
                {
                    User = new User
                    {
                        UserId = d2.sender.user_id,
                        UserName = d2.sender.user_name,
                        ImageSource = d2.sender.header
                    },
                    Content = d2.content,
                    NotificationTime = UnixTimeStampToDateTime(double.Parse(d2.create_time.ToString()))
                };
                notifications.Add(notification);
            }
        }

        public static async Task GetRecordAwesomeAsync(ObservableCollection<User> awesomeUsers, string recordId)
        {
            var url = ServerUrl + GoalRecordAwesome + recordId;
            var result = await GetBaseAsync(url);
            if (string.IsNullOrEmpty(result)) return;

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);
            //todo:解析数据
            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());
                var user = new User
                {
                    ImageSource = d2.user.header
                };
                awesomeUsers.Add(user);
            }
        }

        public static async Task GetRecordCommentsAsync(ObservableCollection<Comment> commentUsers, string recrodId)
        {
            var url = ServerUrl + GoalRecordComment + recrodId;
            var result = await GetBaseAsync(url);

            dynamic d = JsonConvert.DeserializeObject(result);
            //todo：解析数据
            var list = new List<object>(d);
            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());

                var comment = new Comment
                {
                    User = new User
                    {
                        ImageSource = d2.user.header,
                        UserName = d2.user.user_name
                    },
                    CommentContent = d2.content
                };
                commentUsers.Add(comment);
            }
        }

        public static async Task GetGoalJoinRecordAsync(ObservableCollection<UserRecord> records, string goalId, string token)
        {
            var url = ServerUrl + GoalJoinRecord + goalId;
            var header = new Dictionary<string, string>();
            header.Add("Authorization", token);

            var result = await GetBaseAsync(url, header);
            if (string.IsNullOrEmpty(result)) return;

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            if (list.Count == 0) return;

            foreach (var record in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(record.ToString());

                var commentsList = new List<object>(d2.comments);
                var awesomesList = new List<object>(d2.awesomes);
                var content = d2.content.ToString();
                var goalRecordId = d2.goal_record_id.ToString();
                var dateTime = UnixTimeStampToDateTime(double.Parse(d2.create_time.ToString()));

                var userRecord = new UserRecord()
                {
                    User = Global.LoginUser,
                    RecordContent = content,
                    GoalId = goalId,
                    AwesomeUsers = GetAwesomeObservableCollection(awesomesList),
                    Comments = GetCommentsObservableCollection(commentsList),
                    RecordTime = dateTime,
                    GoalRecordId = goalRecordId
                };
                records.Insert(0, userRecord);
            }
        }

        public static async Task GetFightingCenter(ObservableCollection<UserRecord> records, string token)
        {
            var url = ServerUrl + FightingCenter;
            var header = new Dictionary<string, string>();
            header.Add("Authorization", token);

            var result = await GetBaseAsync(url, header);
            if (string.IsNullOrEmpty(result)) return;

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var record in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(record.ToString());

                var commentsList = new List<object>(d2.comments);
                var awesomesList = new List<object>(d2.awesomes);
                var content = d2.content.ToString();
                var goalRecordId = d2.goal_record_id.ToString();
                var goalId = d2.goal_id;
                //bool canAwesome = d2.can_awesome;
                //var goalName = d2.goal_name;
                var dateTime = UnixTimeStampToDateTime(double.Parse(d2.create_time.ToString()));

                var userRecord = new UserRecord()
                {
                    User = Global.LoginUser,
                    RecordContent = content,
                    GoalId = goalId,
                    AwesomeUsers = GetAwesomeObservableCollection(awesomesList),
                    Comments = GetCommentsObservableCollection(commentsList),
                    RecordTime = dateTime,
                    GoalRecordId = goalRecordId
                };
                records.Insert(0, userRecord);
            }
        }

        public static async Task GetAllRecordsForGoalAsync(ObservableCollection<UserRecord> records, string goalId)
        {
            var url = ServerUrl + GoalRecordList + goalId;
            var result = await GetBaseAsync(url);
            if (string.IsNullOrEmpty(result)) return;

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var record in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(record.ToString());

                var commentsList = new List<object>(d2.comments);
                var awesomesList = new List<object>(d2.awesomes);
                var content = d2.content.ToString();
                var goalRecordId = d2.goal_record_id.ToString();
                var userHeader = d2.publisher.header.ToString();
                var userId = d2.publisher.user_id.ToString();
                var userName = d2.publisher.user_name.ToString();
                //bool canAwesome = d2.can_awesome;
                //var goalName = d2.goal_name;
                var dateTime = UnixTimeStampToDateTime(double.Parse(d2.create_time.ToString()));

                var userRecord = new UserRecord()
                {
                    User = new User
                    {
                        ImageSource = userHeader,
                        UserId = userId,
                        UserName = userName
                    },
                    RecordContent = content,
                    GoalId = goalId,
                    AwesomeUsers = GetAwesomeObservableCollection(awesomesList),
                    Comments = GetCommentsObservableCollection(commentsList),
                    RecordTime = dateTime,
                    GoalRecordId = goalRecordId
                };
                records.Insert(0, userRecord);
            }
        }

        public static async Task GetGoalDetailAsync(ObservableCollection<UserRecord> records, string goalId)
        {
            var url = ServerUrl + GoalDetail + goalId;

            var result = await GetBaseAsync(url);
            if (string.IsNullOrEmpty(result)) return;

            dynamic d = JsonConvert.DeserializeObject(result);

            ViewModelLocator.GoalDetailViewModel.Description = d.description;
            ViewModelLocator.GoalDetailViewModel.ImageUrl = d.image;

            var recordList = new List<object>(d.goal_records);
            foreach (var record in recordList)
            {
                dynamic d2 = JsonConvert.DeserializeObject(record.ToString());

                var commentsList = new List<object>(d2.comments);
                var awesomesList = new List<object>(d2.awesomes);
                var content = d2.content.ToString();
                var goalRecordId = d2.goal_record_id.ToString();
                var dateTime = UnixTimeStampToDateTime(double.Parse(d2.create_time.ToString()));
                var user = new User
                {
                    ImageSource = d2.publisher.header,
                    UserName = d2.publisher.user_name,
                    UserId = d2.publisher.user_id
                };

                var userRecord = new UserRecord()
                {
                    //todo:改为recor对应的user
                    User = user,
                    RecordContent = content,
                    GoalId = goalId,
                    AwesomeUsers = GetAwesomeObservableCollection(awesomesList),
                    Comments = GetCommentsObservableCollection(commentsList),
                    RecordTime = dateTime,
                    GoalRecordId = goalRecordId,
                    AllAwesomeCount = awesomesList.Count,
                    AllCommentsCount = commentsList.Count
                };
                records.Insert(0, userRecord);
            }
        }

        public static async Task GetOtherFightingCenter(ObservableCollection<UserRecord> records, string token,
            string userId)
        {
            var url = ServerUrl + string.Format(OtherFightingCenter,userId);
            var header = new Dictionary<string, string>();
            header.Add("Authorization", token);

            var result = await GetBaseAsync(url, header);
            if (string.IsNullOrEmpty(result)) return;

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var record in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(record.ToString());

                var commentsList = new List<object>(d2.comments);
                var awesomesList = new List<object>(d2.awesomes);
                var content = d2.content.ToString();
                var goalRecordId = d2.goal_record_id.ToString();
                var goalId = d2.goal_id;
                //bool canAwesome = d2.can_awesome;
                var dateTime = UnixTimeStampToDateTime(double.Parse(d2.create_time.ToString()));

                var userRecord = new UserRecord()
                {
                    User = Global.LoginUser,
                    RecordContent = content,
                    GoalId = goalId,
                    AwesomeUsers = GetAwesomeObservableCollection(awesomesList),
                    Comments = GetCommentsObservableCollection(commentsList),
                    RecordTime = dateTime,
                    GoalRecordId = goalRecordId
                };
                records.Insert(0, userRecord);
            }
        }

        public static async Task GetOtherFollowersAsync(string token, ObservableCollection<User> followers, string userId)
        {
            var url = ServerUrl + string.Format(OtherFollower, userId);
            var header = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());
                var follower = new User
                {
                    UserId = d2.user_id,
                    UserName = d2.user_name,
                    ImageSource = d2.header
                };
                followers.Add(follower);
            }
        }

        public static async Task GetOtherFollowingsAsync(string token, ObservableCollection<User> followings,
            string userId)
        {
            var url = ServerUrl + string.Format(OtherFollowing, userId);
            var header = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var result = await GetBaseAsync(url, header);

            dynamic d = JsonConvert.DeserializeObject(result);
            var list = new List<object>(d);

            foreach (var o in list)
            {
                dynamic d2 = JsonConvert.DeserializeObject(o.ToString());
                var following = new User
                {
                    UserId = d2.user_id,
                    UserName = d2.user_name,
                    ImageSource = d2.header
                };
                followings.Add(following);
            }
        }

        /// <summary>
        /// 提供访问服务器API的GET方法
        /// </summary>
        /// <param name="url">需要GET的URL地址</param>
        /// <param name="headers">GET时需要的header信息</param>
        /// <returns>从服务器获取的json字符串</returns>
        private static async Task<string> GetBaseAsync(string url, Dictionary<string, string> headers = null)
        {
            var request = WebRequest.CreateHttp(new Uri(url + "?t=" + DateTime.Now.ToString("g")));

            request.Method = Get;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            var responseTask = new TaskCompletionSource<string>();
            request.BeginGetResponse(ar =>
            {
                try
                {
                    var requestState = (HttpWebRequest)ar.AsyncState;
                    var webResponse = requestState.EndGetResponse(ar);
                    using (var stream = webResponse.GetResponseStream())
                    {
                        var sr = new StreamReader(stream);
                        var str = sr.ReadToEnd();
                        responseTask.SetResult(HttpUtility.UrlDecode(str));
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("获取Get返回值错误:{0}", e.Message);
                    responseTask.SetResult(string.Empty);
                }

            }, request);

            return await responseTask.Task;

        }

        #endregion

        #region POST方法

        public static async Task<string> LoginAsync(string name, string description, string facebookToken, string header)
        {
            var url = ServerUrl + Login;
            var headers = new Dictionary<string, string>
            {
               {"name",name},
               {"description",description},
               {"facebooktoken",facebookToken},
               {"headerurl",header}
            };
            //var postData = new Dictionary<string, string>
            //{
            //    {"test","test"}
            //};

            var responseJson = await PostBaseAsync(url, "", headers);

            dynamic d = JsonConvert.DeserializeObject(responseJson);
            return d.token.ToString();
        }

        public static async Task<string> PostRecordAsync(string content, string goalId, string token)
        {
            var url = ServerUrl + GoalRecord;
            var headers = new Dictionary<string, string>
            {
                { "Authorization", token }, 
                { "content", content }, 
                { "goalid", goalId }
            };

            var responseJson = await PostBaseAsync(url, "", headers);

            if (string.IsNullOrEmpty(responseJson)) return string.Empty;

            dynamic d = JsonConvert.DeserializeObject(responseJson);

            return d.result;
        }

        public static async Task<string> PostRecordAwesomeAsync(string recordId, string token)
        {
            var url = ServerUrl + GoalRecordAwesome + recordId;
            var headers = new Dictionary<string, string>
            {
                {"Authorization", token},
            };

            var responseJson = await PostBaseAsync(url, "", headers);

            if (string.IsNullOrEmpty(responseJson)) return string.Empty;

            dynamic d = JsonConvert.DeserializeObject(responseJson);

            return d.awesome_id;
        }

        public static async Task<string> PostRecordCommentAsync(string recordId, string token, string content)
        {
            var url = ServerUrl + GoalRecordComment + recordId;
            var headers = new Dictionary<string, string>
            {
                {"Authorization", token},
                {"content", content}
            };

            var responseJson = await PostBaseAsync(url, "", headers);

            if (string.IsNullOrEmpty(responseJson)) return string.Empty;

            dynamic d = JsonConvert.DeserializeObject(responseJson);

            return HttpUtility.UrlDecode(d.content.ToString());
        }

        public static async Task<string> PostEncourgeAsync(string token, string userId)
        {
            var url = ServerUrl + Encourage + userId;
            var headers = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var responseJson = await PostBaseAsync(url, "", headers);
            if (string.IsNullOrEmpty(responseJson)) return string.Empty;

            dynamic d = JsonConvert.DeserializeObject(responseJson);

            return d.result;
        }

        public static async Task<string> PostFollowAsync(string token, string userId)
        {
            var url = ServerUrl + Follow + userId;
            var headers = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var responseJson = await PostBaseAsync(url, "", headers);
            if (string.IsNullOrEmpty(responseJson)) return string.Empty;

            dynamic d = JsonConvert.DeserializeObject(responseJson);

            return d.result;
        }

        public static async Task<string> PostUnFollowAsync(string token, string userId)
        {
            var url = ServerUrl + UnFollow + userId;
            var headers = new Dictionary<string, string>
            {
                {"Authorization", token}
            };
            var responseJson = await PostBaseAsync(url, "", headers);
            if (string.IsNullOrEmpty(responseJson)) return string.Empty;

            dynamic d = JsonConvert.DeserializeObject(responseJson);

            return d.result;
        }

        public static async Task<string> PostUserProfileAsync(string token, string userName, string description)
        {
            var url = ServerUrl + User;
            var headers = new Dictionary<string, string>
            {
                {"Authorization", token},
                {"name",userName},
                {"description",description}
            };
            var responseJson = await PostBaseAsync(url, "", headers);
            if (string.IsNullOrEmpty(responseJson)) return string.Empty;

            dynamic d = JsonConvert.DeserializeObject(responseJson);

            return d.result;
        }

        /// <summary>
        /// 提供访问服务器API的POST方法
        /// </summary>
        /// <param name="url">需要POST的URL地址</param>
        /// <param name="postData">需要POST的数据</param>
        /// <param name="headers">POST时需要的header信息</param>
        /// <returns></returns>
        private static async Task<string> PostBaseAsync(string url, string postData, Dictionary<string, string> headers)
        {
            //try
            //{
            //    var httpClient = new HttpClient(new HttpClientHandler());
            //    if (headers != null)
            //    {
            //        //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //        foreach (var header in headers)
            //        {
            //            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            //        }
            //    }

            //    var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(postData));
            //    response.EnsureSuccessStatusCode();
            //    var responseJson = await response.Content.ReadAsStringAsync();

            //    return responseJson;
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e.Message);
            //    return string.Empty;
            //}
            _waitSignal.Reset();

            var requestTask = new TaskCompletionSource<string>();
            var requestFail = false;

            var httpRequest = WebRequest.CreateHttp(url);
            httpRequest.Method = Post;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpRequest.Headers[header.Key] = HttpUtility.UrlEncode(header.Value);
                }
            }

            httpRequest.BeginGetRequestStream(ar =>
            {
                try
                {
                    var request = (HttpWebRequest)ar.AsyncState;
                    using (var requestStream = request.EndGetRequestStream(ar))
                    {
                        var data = Encoding.UTF8.GetBytes(postData);
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();
                        _waitSignal.Set();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("获取POST流错误:{0}", e.Message);
                    Task.Delay(200);
                    requestTask.SetResult(string.Empty);
                    requestFail = true;
                    _waitSignal.Set();

                }
            }, httpRequest);

            _waitSignal.WaitOne();

            if (!requestFail)
            {
                httpRequest.BeginGetResponse(ar =>
                {
                    try
                    {
                        var request = (HttpWebRequest)ar.AsyncState;
                        var response = request.EndGetResponse(ar);
                        using (var stream = response.GetResponseStream())
                        {
                            var sr = new StreamReader(stream);
                            var resultJson = sr.ReadToEnd();
                            requestTask.SetResult(resultJson);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("获取post返回值错误:{0}", e.Message);
                        requestTask.TrySetResult(string.Empty);
                    }

                }, httpRequest);
            }


            return await requestTask.Task;
        }

        #endregion

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static ObservableCollection<User> GetAwesomeObservableCollection(List<object> list)
        {
            if (!list.Any())
            {
                return new ObservableCollection<User>();
            }

            var userList = new List<User>();
            foreach (var o in list)
            {
                dynamic d = JsonConvert.DeserializeObject(o.ToString());
                var user = new User
                {
                    ImageSource = d.header,
                    UserId = d.user_id,
                    UserName = d.user_name
                };
                userList.Add(user);
            }

            return new ObservableCollection<User>(userList);
        }

        private static ObservableCollection<Comment> GetCommentsObservableCollection(List<object> list)
        {
            if (!list.Any())
            {
                return new ObservableCollection<Comment>();
            }

            var userList = new List<Comment>();
            foreach (var o in list)
            {
                dynamic d = JsonConvert.DeserializeObject(o.ToString());
                var comment = new Comment
                {
                    User = new User
                    {
                        UserId = d.user.user_id,
                        UserName = d.user.user_name,
                        ImageSource = d.user.header,
                    },
                    CommentContent = d.content.ToString()
                };
                userList.Add(comment);
            }

            return new ObservableCollection<Comment>(userList);
        }
    }
}
