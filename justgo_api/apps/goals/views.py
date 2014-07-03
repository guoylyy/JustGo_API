from django.views.decorators.csrf import csrf_exempt
from django.http import HttpResponse
from goal.models import *
from user.models import *
from uploadfile.models import *
from config import get_config,get_page_result
from helper import check_token
import simplejson
import datetime
import time

def get_all_category(request):
    '''
    return json list of all names of category
    '''
    try :
        cat_names = Category.objects.all().values('id', 'name')
        return HttpResponse(get_page_result('200',simplejson.dumps(list(cat_names))))
    except Exception as e:
        print(e)
        return HttpResponse(get_page_result('500'))

def get_goal_list(request):
    '''
    return json list of all names of goals to specilized category
    [ {"id":value, "name":value, "detail":value}, {"id":value, "name":value, "detail":value}, ... ]
    '''
    try :
        cat_id = request.GET.get('category_id')
        cat = Category.objects.filter(id=cat_id)
        if len(cat)==0:
            return HttpResponse(get_page_result('031'))
        goal_names = Goal.objects.filter(category=cat[0]).values('id', 'name', 'detail')
        return HttpResponse(get_page_result('200',simplejson.dumps(list(goal_names))))
    except Exception as e:
        print(e)
        return HttpResponse(get_page_result('500'))

def get_goal_list_all(request):
    '''
    return json list of all names of goals to specilized category
    [ {"id":value, "name":value, "detail":value}, {"id":value, "name":value, "detail":value}, ... ]
    '''
    try :
        goals = Goal.objects.all().values('id', 'name', 'detail')
        return HttpResponse(get_page_result('200',simplejson.dumps(list(goals))))
    except Exception as e:
        return HttpResponse(get_page_result('500'))

@csrf_exempt
def join_goal(request):
    '''
    return success if join successfully
    '''
    try :
        token = request.POST.get('token')
        goal_id = request.POST.get('goal_id')
        privacy = request.POST.get('privacy')
        response = check_token(token)
        if response is True:
            goal = Goal.objects.get(id=goal_id)
            user_id = token.split(':')[-1]
            user = User.objects.get(id=user_id)
            if(len(UserGoal.objects.filter(user=user,goal=goal))==0):
                user_goal = UserGoal(user=user, goal=goal, privacy=privacy)
                user_goal.save()
            return HttpResponse(get_page_result('200'))
        else :
            return response
    except Exception as e :
        print (e)
        return HttpResponse(get_page_result('500'))

#to post
@csrf_exempt
def exit_goal(request):
    '''
    return success if exit successfully
    '''
    try :
        token = request.POST.get('token')
        goal_id = request.POST.get('goal_id')
        response = check_token(token)
        if response is True:
            goal = Goal.objects.filter(id=goal_id)[0]
            user_id = token.split(':')[-1]
            user = User.objects.filter(id=user_id)[0]
            UserGoal.objects.filter(user=user, goal=goal).delete()
            return HttpResponse(get_page_result('200'))
        else :
            return response
    except Exception as e :
        print (e)
        return HttpResponse(get_page_result('500'))

# TODO: add picture upload function
@csrf_exempt
def sync_push(request):
    '''
    return success if push successfully
    '''
    try :
        token = request.GET.get('token')
        data = request.GET.get('data')
        response = check_token(token)
        if response is True:
            user_id = token.split(':')[-1]
            check_list = simplejson.loads(data)
            print (check_list)
            for item in check_list:
                user = User.objects.filter(id=user_id)[0]
                goal = Goal.objects.filter(id=item['goal_id'])[0]
                user_goal = UserGoal.objects.filter(user=user, goal=goal)[0]
                check_date = datetime.datetime.fromtimestamp(int(item['check_date']))
                UserGoalCheckout(user_goal=user_goal, comment=item['comment'], date_check=check_date).save()
            return HttpResponse(get_page_result('200'))
        else :
            return response
    except Exception as e :
        print (e)
        return HttpResponse(get_page_result('500'))


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
        response = check_token(token)
        if response is True:
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
            return response
    except Exception as e :
        print (e)
        return HttpResponse(get_page_result('500'))

@csrf_exempt
def make_comment(request):
    '''
    return success if insert into DB successfully
    '''
    try :
        token = request.POST.get('token')
        checkout_id = request.POST.get('checkout_id')
        comment = request.POST.get('comment')
        response = check_token(token)
        if response is True:
            user_id = token.split(':')[-1]
            uuser = User.objects.filter(id=user_id)[0]
            checkout = UserGoalCheckout.objects.filter(id=checkout_id)[0]
            UserGoalComment(checkout = checkout, comment = comment, comment_user = uuser).save()
            return HttpResponse(get_page_result('200'))
        else :
            return response
    except Exception as e :
        print (e)
        return HttpResponse(get_page_result('500'))

@csrf_exempt
def make_awesome(request):
    '''
    return success if insert into DB successfully
    '''
    try :
        token = request.POST.get('token')
        checkout_id = request.POST.get('checkout_id')
        response = check_token(token)
        if response is True:
            user_id = token.split(':')[-1]
            uuser = User.objects.filter(id=user_id)[0]
            checkout = UserGoalCheckout.objects.filter(id=checkout_id)[0]
            UserGoalAwesome(checkout = checkout, awesome_user = uuser).save()
            return HttpResponse(get_page_result('200'))
        else :
            return response
    except Exception as e :
        print (e)
        return HttpResponse(get_page_result('500'))

# TODO : to be finished
def goal_status(request):
    '''
    '''
    try :
        checkout_id = request.GET.get('checkout_id')
        return HttpResponse(get_page_result('200'))
    except Exception as e :
        print (e)
        return HttpResponse(get_page_result('500'))