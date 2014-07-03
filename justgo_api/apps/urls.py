import os.path
from django.conf.urls import patterns, include, url


APP_PATH = os.path.dirname(__file__)

user_patterns = patterns('apps.users.views',
    url(r'^user/register$', 'register'),
    url(r'^user/login$', 'login'),
    url(r'^user/login_status$', 'login_status'),
    url(r'^user/logout$', 'logout'),
    url(r'^user/data_pull$', 'data_pull'),
)

goal_patterns = patterns('apps.goals.views',
    url(r'^goal/category$', 'get_all_category'),
    url(r'^goal/goal_list$', 'get_goal_list'),
    url(r'^goal/goal_list_all$', 'get_goal_list_all'),
    url(r'^goal/join$', 'join_goal'),
    url(r'^goal/exit$', 'exit_goal'),
    url(r'^goal/sync_push$', 'sync_push'),
    url(r'^goal/sync_pull$', 'sync_pull'),
    url(r'^goal/make_comment$', 'make_comment'),
    url(r'^goal/make_awesome$', 'make_awesome'),
    url(r'^goal/status$', 'goal_status'),
)

urlpatterns = user_patterns + goal_patterns