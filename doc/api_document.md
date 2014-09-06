The api requirement of WP.

---
###0. 系统参数

```
基地址：http://tjcsdc.com/V1/
数据库信息：
```

###0.2 系统错误消息代码
系统消息代码是系统前后台用户通信错误信息的口令，可以方便统一消息意义



##1. Users API

###1.1 登陆API
用户登陆，即重新刷新token，依然用已经fackbook账户登录。 需要调用facebook api做认证. 这里用户需要

```
url: login
method: post
```

####数据说明

```
发送数据
	name: "xxx"
	description: "xxxx"
	facebook_token : "fdsfsa" //唯一标示一个facebook用户，传facebook id过来

返回数据
	{"token":"1d859c9ea10b754d1686e89423291399:1406980314:1"}

```


###1.2 用户个人数据API
获取和更新用户基本信息（头像、用户名、昵称以及个人简介）

这里应该是都需要传Authoriation的。

同时也获取单个用户的关注列表和被关注列表

```
url: user | user/<user_id>
method: get | post
```
####数据说明-获取 GET
```
发送数据
	无，必须访问 user/<user_id>接口，否则报错
	
	headers:
		- Authorization : <token>

返回数据：
{
    "description": "I am a BOY",
    "fans": [
        {
            "header": "fdsafasf",
            "user_id": 2,
            "user_name": "yilianggggg"
        }
    ],
    "followings": [
        {
            "header": "fdsafasf",
            "user_id": 2,
            "user_name": "yilianggggg"
        }
    ],
    "name": "yiliangg",
    "user_id": 1,
    "header_icon": "http:///Users/globit/git/JustGo_Project/go_api/userimages/user-header/1/0/1.48x52.jpe?_ts=20140906050939000000"
}


```

####数据说明-更新 POST
更新用户基本信息

```
发送数据
	headers: 
		Authorization : <token>
	form:
		name: “fsafa”
		description: "fsafsa"

返回数据：
{
    "description": "I am a",
    "fans": [
        {
            "header": "fdsafasf",
            "user_id": 2,
            "user_name": "yilianggggg"
        }
    ],
    "followings": [
        {
            "header": "fdsafasf",
            "user_id": 2,
            "user_name": "yilianggggg"
        }
    ],
    "name": "yiliangg",
    "user_id": 1,
    "header_icon": "file:///Users/globit/git/JustGo_Project/go_api/userimages/user-header/1/0/1.48x52.jpe?_ts=20140906050939000000"
}
```


###1.3 查询用户API
输入用户名，进行模糊匹配查询
//未开发 待开发


###1.4 关注API
关注某用户

```
url: /user/follow/<int:user_id> //被关注用户的ID
method:post
```
####数据说明

```
发送数据
	headers:
		-Authorization : 'dfasfsa'		//当前用户的authorisation
		
返回数据：
{
    "result": "success"
}
	
```


###1.5 取消关注API
取消关注


```
url: /user/unfollow/<int:user_id> //被取消关注用户的ID
method:post
```
####数据说明

```
发送数据
	headers:
		-Authorization : 'dfasfsa'		//当前用户的authorisation
		
返回数据：
{
    "result": "success"
}
	
```


##2. Goals API

###2.1 同步目标API
(未完成) 

###2.2 获取目标所有Category
获取各个category的名字和描述，这个描述就是主键

```
url: goal_category
method: get
```
####数据说明
```
发送数据
	无

返回数据：
[
    {
        "category_name": "Popular",
        "desciprtion": ""
    },
    ......
    ,
    {
        "category_name": "Learning",
        "desciprtion": ""
    }
]
```

###2.3 通过CATEGORY NAME 获取Goals
获取goal list

```
url: goal/<category_name>
method: get
```
####数据说明

```
发送数据
	无
返回数据：
[
    {
        "desciprtion": null,
        "goal_id": 1,
        "goal_name": "Drink water",
        "image": "file:///Users/globit/git/JustGo_Project/go_api/userimages/goal-image/1/0/1.301x328.jpe?_ts=20140906060541000000",
        "joins": 0
    }.....
    
]
```

###2.4 添加和获取goal的record
添加关于某个goal 的 record

```
url: goal_record | goal_record/<record_id>
method: post | get
```


####数据说明 GET
通过goal record id 获取特定record

使用goal_record/<record_id>作为url

```
发送数据
	headers:
		-Authorization : 'dfasfsa'		
		
返回数据：
{
    "awesomes": [
        {
            "awesome_id": 1,
            "goal_record_id": 1,
            "user_id": 1
        }
    ],
    "comments": [
        {
            "comment_id": 1,
            "content": "This is a test",
            "create_time": 1409423223,
            "goal_record_id": 1,
            "user_id": 1
        }
    ],
    "content": "yiliang",
    "goal_id": 1,
    "goal_record_id": 1,
    "image": "fdsafa"
}
```


####数据说明 POST

使用 goal_record 作为url
```
发送数据
	headers:
		-Authorization : 'dfasfsa'
	form:
		-content : 'fsdfas'
		-goal_id : 1
		
返回数据：
{
    "result": "success"
}
```


###2.4 查看当前用户在某一个goal下所有的record
RT，awesomes和comments获取前5个

```
url: goal_join_record/<record_id>
method: get
```
####数据说明

```
发送数据
	headers:
		-Authorization : 'dfasfsa'

返回数据：
[
    {
        "awesomes": [],
        "comments": [],
        "content": "yiliang",
        "goal_id": 1,
        "goal_record_id": 2,
        "image": "fdsafa"
    }
]
```


###2.5 为Record添加comment 以及获取 comments
给 record添加comment
获取record所有的comments

```
url:  goal_record_comment/<record_id>
method: get | post
```
####数据说明 GET

```
发送数据
	无		

返回数据：
[
    {
        "comment_id": 1,
        "content": "This is a test",
        "create_time": 1409423223,
        "goal_record_id": 1,
        "user_id": 1
    }
]
```

####数据说明 POST
```
发送数据
	headers:
		-Authorization : 'dfasfsa'
	form:
		-content : 'fsdfas'

返回数据：
[
    {
        "comment_id": 1,
        "content": "This is a test",
        "create_time": 1409423223,
        "goal_record_id": 1,
        "user_id": 1
    }
]
```

###2.6 点赞
点赞接口：点赞和获取一个record所有的赞

```
url: goal_record_awesome/<record_id>
method: get | post
```
####数据说明 GET
```
发送数据
	无

返回数据：
[
    {
        "awesome_id": 1,
        "goal_record_id": 1,
        "user_id": 1   //这里应该返回头像和id的
    }
]
```

####数据说明 POST
```
发送数据
	headers:
		-Authorization : 'dfasfsa'

返回数据：
[
    {
        "awesome_id": 1,
        "goal_record_id": 1,
        "user_id": 1   //这里应该返回头像和id的
    }
]
```

###2.7 拉取用户所有join的goals情况
获取用户join的goal

```
url: goal_join
method: get
```
####数据说明 GET
```
发送数据
	headers:
		-Authorization : 'dfasfsa'
		
返回数据：
[
    {
        "create_time": 1409985233,
        "end_date": "2014-08-20",
        "frequency": "1,2,3,4,5",
        "goal_id": 1,
        "goal_join_id": "fs332ab",
        "is_finished": false,
        "is_reminder": true,
        "reminder_time": "12:02:00",
        "start_date": "2014-08-08",
        "time_span": 7,
        "update_time": 1409985233,
        "user_id": 1
    }
]
```

###2.6 拉取用户所有goal的 track记录
获取用户所有的track

```
url: goal_join_track
method: get
```
####数据说明 GET
```
发送数据
	headers:
		-Authorization : 'dfasfsa'

返回数据：
[
    {
        "create_time": 1409985233,
        "goal_join_id": "fs332ab",
        "goal_track_id": 1,
        "track_date": "2014-08-09",
        "update_time": 1409985233,
        "user_id": 1
    },
    {
        "create_time": 1409985233,
        "goal_join_id": "fs332ab",
        "goal_track_id": 2,
        "track_date": "2014-08-10",
        "update_time": 1409985233,
        "user_id": 1
    }
]
```




##3. Notification API


###3.1 拉取通知
通知分为几类：
    1. 系统通知
    2. follow的通知
    3. encourage通知

封装成一个接口，然后拉取和添加通知

```
url:  notification
method: get
```
####数据说明 GET
```
发送数据
	headers:
		-Authorization : 'dfasfsa'

返回数据：
[
    {
        "create_time": 1409985233,
        "goal_join_id": "fs332ab",
        "goal_track_id": 1,
        "track_date": "2014-08-09",
        "update_time": 1409985233,
        "user_id": 1
    }
]
```


###3.2 Encourage用户
就某好友参加的Goal发送鼓励信息

```
url: 
method: get
```
####数据说明 GET
```
发送数据
	headers:
		-Authorization : 'dfasfsa'

返回数据：
[
    {
        "create_time": 1409985233,
        "goal_join_id": "fs332ab",
        "goal_track_id": 1,
        "track_date": "2014-08-09",
        "update_time": 1409985233,
        "user_id": 1
    }
]
```











