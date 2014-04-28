from django.views.decorators.csrf import csrf_exempt
from django.http import HttpResponse
from goal.models import *
from user.models import *
from uploadfile.models import *
from config import get_config
import simplejson
import datetime
import time

# Create your views here.
def get_all_category(request):
    '''
    return json list of all names of category
    '''
    try :
        token = request.GET.get('token')
        cat_names = Category.objects.all().values('id', 'name')
        return HttpResponse(simplejson.dumps(list(cat_names)))
    except Exception as e:
        print(e)
        return HttpResponse(__get_result('fail'))


def get_goal_list(request):
    '''
    return json list of all names of goals to specilized category
    [ {"id":value, "name":value, "detail":value}, {"id":value, "name":value, "detail":value}, ... ]
    '''
    try :
        cat_id = request.GET.get('category_id')
        cat = Category.objects.filter(id=cat_id)
        if len(cat)==0:
            return HttpResponse(__get_result('wrong category name'))
        goal_names = Goal.objects.filter(category=cat[0]).values('id', 'name', 'detail')
        return HttpResponse(simplejson.dumps(list(goal_names)))
    except Exception as e:
        print(e)
        return HttpResponse(__get_result('fail'))

def get_goal_list_all(request):
    '''
    return json list of all names of goals to specilized category
    [ {"id":value, "name":value, "detail":value}, {"id":value, "name":value, "detail":value}, ... ]
    '''
    try :
        goals = Goal.objects.all().values('id', 'name', 'detail')
        return HttpResponse(simplejson.dumps(list(goals)))
    except Exception as e:
        return HttpResponse(__get_result('fail'))

@csrf_exempt
def join_goal(request):
    '''
    return success if join successfully
    '''
    try :
        token = request.GET.get('token')
        goal_id = request.GET.get('goal_id')
        privacy = request.GET.get('privacy')

        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))

            goal = Goal.objects.filter(id=goal_id)[0]
            user_id = token.split(':')[-1]
            user = User.objects.filter(id=user_id)[0]
            user_goal = UserGoal(user=user, goal=goal, privacy=privacy)
            user_goal.save()
            return HttpResponse(__get_result('success'))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e :
        print (e)
        return HttpResponse(__get_result('fail'))

#to post
def exit_goal(request):
    '''
    return success if exit successfully
    '''
    try :
        token = request.GET.get('token')
        goal_id = request.GET.get('goal_id')

        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))

            goal = Goal.objects.filter(id=goal_id)[0]
            user_id = token.split(':')[-1]
            user = User.objects.filter(id=user_id)[0]
            UserGoal.objects.filter(user=user, goal=goal).delete()
            return HttpResponse(__get_result('success'))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e :
        print (e)
        return HttpResponse(__get_result('fail'))

# TODO: add picture upload function
@csrf_exempt
def sync_push(request):
    '''
    return success if push successfully
    '''
    try :
        token = request.GET.get('token')
        data = request.GET.get('data')

        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))

            user_id = token.split(':')[-1]
            check_list = simplejson.loads(data)
            print (check_list)
            for item in check_list:
                user = User.objects.filter(id=user_id)[0]
                goal = Goal.objects.filter(id=item['goal_id'])[0]
                user_goal = UserGoal.objects.filter(user=user, goal=goal)[0]
                check_date = datetime.datetime.fromtimestamp(int(item['check_date']))
                UserGoalCheckout(user_goal=user_goal, comment=item['comment'], date_check=check_date).save()
            return HttpResponse(__get_result('success'))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e :
        print (e)
        return HttpResponse(__get_result('fail'))


# TODO: generate picture path 
@csrf_exempt
def sync_pull(request):
    '''
    return json array of all the goals and status
    [ {
       "goal_id" : id, 
       "goal_name" : name, 
       "check_records":
       [ { 
           "check_id" : id,
           "check_date" : date(ms), 
           "comment" : date, 
           "picture" : pic_path
         },
         ... ]
      },
      ... ]
    '''
    try :
        token = request.POST.get('token')

        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))

            user_id = token.split(':')[-1]
            uuser = User.objects.filter(id=user_id)[0]
            all_user_goals = UserGoal.objects.filter(user=uuser)
            res_data = []
            for user_goal in all_user_goals:
                tmp_dict = {}
                goal = user_goal.goal
                tmp_dict['goal_id'] = goal.id
                tmp_dict['goal_name'] = goal.name
                l = []
                check_list = UserGoalCheckout.objects.filter(user_goal = user_goal)
                for c in check_list:
                    check_dict = {}
                    check_dict['id'] = c.id
                    check_dict['check_date'] = c.date_check.timestamp()
                    check_dict['comment'] = c.comment
                    check_dict['picture'] = c.picture
                    l.append(check_dict)
                tmp_dict['check_records'] = l
                res_data.append(tmp_dict)
            return HttpResponse(simplejson.dumps(res_data))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e :
        print (e)
        return HttpResponse(__get_result('fail'))

@csrf_exempt
def make_comment(request):
    '''
    return success if insert into DB successfully
    '''
    try :
        token = request.POST.get('token')
        checkout_id = request.POST.get('checkout_id')
        comment = request.POST.get('comment')

        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))

            user_id = token.split(':')[-1]
            uuser = User.objects.filter(id=user_id)[0]
            checkout = UserGoalCheckout.objects.filter(id=checkout_id)[0]
            UserGoalComment(checkout = checkout, comment = comment, comment_user = uuser).save()
            return HttpResponse(__get_result('success'))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e :
        print (e)
        return HttpResponse(__get_result('fail'))

@csrf_exempt
def make_awesome(request):
    '''
    return success if insert into DB successfully
    '''
    try :
        token = request.POST.get('token')
        checkout_id = request.POST.get('checkout_id')

        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))

            user_id = token.split(':')[-1]
            uuser = User.objects.filter(id=user_id)[0]
            checkout = UserGoalCheckout.objects.filter(id=checkout_id)[0]
            UserGoalAwesome(checkout = checkout, awesome_user = uuser).save()
            return HttpResponse(__get_result('success'))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e :
        print (e)
        return HttpResponse(__get_result('fail'))

# TODO : to be finished
def goal_status(request):
    '''
    '''
    try :
        checkout_id = request.GET.get('checkout_id')
        return HttpResponse(__get_result('success'))
    except Exception as e :
        print (e)
        return HttpResponse(__get_result('fail'))


###############################################################
## private functions start here

def __get_result(result):
    return str("{\"result\":\""+result+"\"}")

def __get_error(msg_num):
    return str("{\"error\":\""+msg_num+"\"}")

def __check_token(token):
    '''
    return
    - True for matched token
    - False for invalid token
    '''
    try :
        md5, expire, id = token.split(":", 3)
        tokens = UserToken.objects.filter(user_id = id)
        if (len(tokens) > 0):
            return True
        print ('Token auth Fail: id %s user token not match' % id)
        return False
    except Exception as e:
        print (e)
        return False

def __check_expire(token):
    '''
    return 
    - True for expire token
    - False for valid token
    - None for error token
    '''
    try :
        md5, expire, id = token.split(":", 3)
        if int(expire) < time.time():
            return False
        return True
    except Exception as e:
        print (e)
        return None

def __get_default_portrait():
    '''
    return UploadFile object
    '''
    try :
        name = get_config('default_portrait_name')
        portrait = UploadFile(file_name=name)
        portrait.save()
        if portrait.id is not None:
            return portrait
        print ('Error: Default portrait save fail!')
        return None
    except Exception as e:
        print (e)
        return None

def __perform_login(user):
    '''
    generate token and return
    '''
    token = __make_token(user.id)
    save_result = __insert_token(user, token)
    if save_result is True:
        return HttpResponse(__get_result(token))
    else:
        print ('Error: register save token fail!')
        return HttpResponse(__get_result('save token fail'))
