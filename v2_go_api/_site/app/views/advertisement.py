# -*- coding: utf-8 -*-

# author: notedit <notedit@gmail.com>
# date: 2012/12/01  morning

import sys 
import time
import flask
from flask import Blueprint, request, g, redirect, Response, current_app, session, \
				jsonify, flash, Module
from flask.views import MethodView
from flask.views import View
from flask import render_template
from flask.ext.login import login_user, current_user, login_required,\
							logout_user
from app.models import Admin, Goal
from app.extensions import login_manager, db
from app.forms import GoalForm

advertisement = Module(__name__)


@advertisement.route('/')
def index():
	return render_template('backend/advertisement.html')


@advertisement.route('/add/')
def add():
	pass

@advertisement.route('/<int:advertisement_id>/edit/')
def edit():
	pass

@advertisement.route('/<int:advertisement_id>/delete/')
def delete():
	pass