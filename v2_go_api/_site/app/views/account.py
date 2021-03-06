# -*- coding: utf-8 -*-

# author: notedit <notedit@gmail.com>
# date: 2012/12/01  morning

import sys 
import time
import flask
from flask import Blueprint
from flask import request
from flask import g
from flask import Module
from flask import redirect
from flask import Response
from flask import current_app
from flask import session
from flask import jsonify
from flask import flash
from flask.views import MethodView
from flask.views import View
from flask import render_template

from flask.ext.login import login_user, current_user, login_required,\
							logout_user

from app.forms.account import LoginForm

from app.models import Admin

from app.extensions import login_manager


instance = Blueprint('account',__name__, static_folder='static')


@login_manager.user_loader
def load_user(id):
    return Admin.query.get(int(id))

class LoginView(MethodView):
    def get(self):
    	form = LoginForm()
    	if not current_user.is_anonymous():
    		return render_template('backend/dashboard.html', form=form)
        return render_template('account/login.html', form=form)

    def post(self):
    	form = LoginForm(request.form)
    	if form.validate_on_submit():
    		admin = Admin.query.filter(Admin.username==form.username.data,\
    			Admin.password==form.password.data).first()
    		if admin:
    			login_user(admin, remember=True)
    			return redirect('/login')
    		else:
				form.message = "Sorry, invalid login!"
    	return render_template('account/login.html', form=form)

class LogoutView(MethodView):
	def get(self):
		logout_user()
		return redirect('/login')

class IndexView(MethodView):
    @login_required
    def get(self):
        return redirect('/login')

instance.add_url_rule('/',view_func=IndexView.as_view('index'),methods=['GET'])
instance.add_url_rule('/login',view_func=LoginView.as_view('login'),methods=['GET','POST'])
instance.add_url_rule('/logout',view_func=LogoutView.as_view('logout'),methods=['GET','POST'])