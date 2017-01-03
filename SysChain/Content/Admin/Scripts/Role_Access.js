$(function(){
	var $columnList = $('.column-list');
	$columnList.on('click', ':checkbox', function(evt) {
		var $input = $(evt.target),
			$dl = $input.closest('dl');

		if ($input.parent('dt').length) {
			$dl.find(':checkbox').prop('checked', $input.is(':checked'));
		} else {
			if ($dl.find('dd > :checkbox:checked').length == 0) {
				$dl.find('dt > :checkbox').removeProp('checked');
			} else {
				$dl.find('dt > :checkbox').prop('checked', true);
			} //if

		} //if
	});
	var $form = $('#frm-role-access'),
		$btn=$(".btn-primary"),
		$cancel=$(".btn-none");
	$form.on('submit', function(evt){
			evt.preventDefault();
			evt.stopPropagation();
			if ($btn.is('.btn-disabled')) {
				return false;
			} 
			$btn.addClass('btn-disabled');
			$cancel.addClass('btn-disabled');
			popup.loading.init();
			$.ajax({  
			    type: "Post",  
			    url: $form.attr("action"),
			    data: $(this).serialize(),  
			    dataType: "json",   
			    success: function (data) {  
			      if (data.Result) {  
					 popup.info.init().show(data.Msg, true,data.Url); 
			      }  
			      else {  
			       popup.info.init().show(data.Msg, false); 
				   $btn.removeClass('btn-disabled');
				   $cancel.removeClass('btn-disabled');
			      }  
			    },   
			    error: function (XMLHttpRequest, textStatus, errorThrown) {  
					$btn.removeClass('btn-disabled');
					$cancel.removeClass('btn-disabled');
			    }  
			}); 
	});
});