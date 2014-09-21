from flask_wtf import Form

class BaseForm(Form):
	message = None
	def __init__(self, *args, **kwargs):
		kwargs['csrf_enabled'] = False
		super(BaseForm, self).__init__(*args, **kwargs)