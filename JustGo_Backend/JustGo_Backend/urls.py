from django.conf.urls import patterns, include, url
import JustGo_Backend.settings as settings
from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'JustGo_Backend.views.home', name='home'),
    # url(r'^blog/', include('blog.urls')),

    #    url(r'^admin/', include(admin.site.urls)),
     url(r'^media/(?P<path>.*)$', 'django.views.static.serve',{'document_root': settings.MEDIA_ROOT }),  
)
