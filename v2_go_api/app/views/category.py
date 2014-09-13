# -*- coding: utf-8 -*-

# author: notedit <notedit@gmail.com>
# date: 2012/12/01  morning

import sys 
import time
import flask
from flask import Blueprint, request, g, redirect, Response, current_app, session, \
				jsonify, flash, Module, url_for
from flask.views import MethodView
from flask.views import View
from flask import render_template
from flask.ext.login import login_user, current_user, login_required,\
							logout_user
from app.models import Admin, Goal, Category
from app.extensions import login_manager, db
from app.forms import GoalForm, CategoryForm

category = Module(__name__)

@category.route('/', methods=('POST','GET'))
def index():
	categorys = Category.query.all()
	form = CategoryForm()
	for c in categorys:
		c.goal_count = len(c.goals)
	return render_template('backend/category.html', categorys=categorys, form=form)

@category.route('/add/', methods=('POST',))
def add():
	form = CategoryForm()
	if form.validate():
		c = Category(form.category_name.data, form.description.data)
		db.session.add(c)
		db.session.commit()
	return redirect(url_for('category.index'))

@category.route('/<string:category_id>/edit/', methods=('POST','GET'))
def edit(category_id):
	c = Category.query.filter(Category.category_name==category_id).first()
	
	return redirect(url_for('category.index'))

@category.route('/<string:category_id>/delete/', methods=('POST','GET'))
def delete(category_id):
	c = Category.query.filter(Category.category_name==category_id).first()
	db.session.delete(c)
	db.session.commit()
	return redirect(url_for('category.index'))