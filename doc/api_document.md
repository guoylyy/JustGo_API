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
    "user_id": 1
}

```

####数据说明-更新 POST
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
    "user_id": 1
}
```


###1.3 查询用户API
输入用户名，进行模糊匹配查询
//未开发



##2. Goals API

###2.1 同步目标API
(未完成) 

###2.2 获取目标所有Category
RT

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
        "desciprtion": "I can naje it ",
        "goal_id": 1,
        "goal_name": "Write A Ariticle",
        "joins": 0
    },
    {
        "desciprtion": "NULfdsafasfL",
        "goal_id": 2,
        "goal_name": "This is a test",
        "joins": 0
    }
]
```

###2.4 当前用户添加关于某个goal的record
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



##3. Notification API


###3.1 拉取通知

未完成







