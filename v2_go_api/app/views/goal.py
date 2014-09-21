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
from sqlalchemy_imageattach.entity import store_context

from app.models import Admin, Goal, Category
from app.extensions import login_manager, db, fs_store
from app.forms import GoalForm

goal =  Module(__name__)

@goal.route('/', methods=("GET","POST"))
def index():
	goals = Goal.query.filter(Goal.is_active==True).all()
	return render_template('backend/goal.html', goals=goals)

@goal.route('/<int:goal_id>/delete/')
def delete(goal_id):
	goal = Goal.query.filter(Goal.goal_id==goal_id).first()
	#if goal:
	#	with store_context(fs_store):
	goal.is_active = False
	db.session.add(goal)
	db.session.commit()
	return redirect('/backend/goal')

@goal.route('/<int:goal_id>/edit/', methods=("GET","POST"))
def edit(goal_id):
	if request.method == 'POST':
		gf = GoalForm()
		names = gf.category_name.data.split(",")
		g = Goal.query.filter(Goal.goal_id==goal_id).first()
		g = gf.to_goal(g)
		for c in g.categories:
			g.categories.remove(c)
		categorys = Category.query.filter(Category.category_name.in_(names)).all()
		if len(categorys) > 0:
			for c in categorys:
				g.categories.append(c)

		with store_context(fs_store):
			if gf.validate():
				g.image.from_file(request.files['image'])
			db.session.add(g)
			db.session.commit()
		return redirect(url_for('goal.edit', goal_id=goal_id))
	elif request.method == 'GET':
		form = GoalForm()
		categorys = Category.query.all()
		names = ",".join([c.category_name for c in categorys])
		goal = Goal.query.filter(Goal.goal_id==goal_id).first()
		form.set_by_goal(goal)
		return render_template('backend/goal-edit.html', goal=goal, category_names=names, form=form)

@goal.route('/add/', methods=("GET","POST"))
def add():
	if request.method == 'POST':
		gf = GoalForm()
		if gf.validate():
			g = Goal(gf.goal_name.data, gf.description.data)
			names = gf.category_name.data.split(",")
			categorys = Category.query.filter(Category.category_name.in_(names)).all()
			if len(categorys) > 0:
				for c in categorys:
					g.categories.append(c)
			with store_context(fs_store):
				g.image.from_file(request.files['image'])
				db.session.add(g)
				db.session.commit()
			return redirect(url_for('goal.index'))
		else:
			return redirect(url_for('goal.add'))
	elif request.method == 'GET':
		form = GoalForm()
		categorys = Category.query.all()
		names = ",".join([c.category_name for c in categorys])
		return render_template('backend/goal-add.html',category_names=names, form=form)

