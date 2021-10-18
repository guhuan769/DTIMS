function submitClick(){
    var isSubmit = true;
    var undefined;
    var element = null;
    $('form input[type=text]').each(function(){
       element=  $(this);
       var req = element.attr('req');
       var text = $(this).val();
       var title=element.attr('title');
       var id = element.attr('id');
       if(req!=undefined){
            if(text == ''){
                $.messager.alert('错误',title + '不能为空','error',function(){
                    element.focus();
                });
                isSubmit = false;
                return false;
            }
       }
       var must = element.attr('must');
       if(must==undefined)must='string';
       switch(must){
            case 'int':
                text = parseInt(text);
                if(!isNaN(text)){
                    var min =  element.attr('min');
                    var max =  element.attr('max');
                    if(isNaN(min)){
                       min =  $('#'+ min).val();
                    }
                    if(isNaN(max)){
                       max =  $('#'+ max).val();
                    }
                    if(min==undefined)min = 0;
                    if(max==undefined)max = 0;
                    if(max == 0)
                    {
                        if(text < parseInt(min)){
                             $.messager.alert('错误',title + '必须大于' + min + ',请核实','error',function(){
                                element.focus();
                             });
                        }
                    }
                    else{
                        if(text < parseInt(min) || text > parseInt(max)){
                            $.messager.alert('错误',title + '的取值范围在' + min + '到' + max + '之间,请核实','error',function(){
                                element.focus();
                            });
                            isSubmit = false;
                            return false;
                        }
                    }
                }
                else{
                        $.messager.alert('错误',title + '只能为数字','error',function(){
                                element.focus();
                        });
                		$(this).val('');
                        isSubmit = false;
                        return false;
                }
            break;
            case 'ints':
                if(!isNaN(text)){
                    var min =  element.attr('minlength');
                    var max =  element.attr('maxlength');
                    if(isNaN(min)){
                       min =  $('#'+ min).val();
                    }
                    if(isNaN(max)){
                       max =  $('#'+ max).val();
                    }
                    if(min==undefined)min = 0;
                    if(max==undefined)max = 60;
                    if(!(text.length >= parseInt(min) &&  text.length <= parseInt(max))){
                        $.messager.alert('错误',title + '的字符长度在' + min + '到' + max + '之间,请核实','error',function(){
                                element.focus();
                        });
                        isSubmit = false;
                        return false;
                    }
                }
                else{
                		alert(title + '只能为数字',function(){
                                element.focus();
                        });
                		$(this).val('');
                        isSubmit = false;
                        return false;
                }
            break;
            case 'string':
                    var minLength =  element.attr('minLength');
                    var maxLength =  element.attr('maxLength');
                    if(minLength==undefined)minLength = 0;
                    if(maxLength==undefined)maxLength = 0;
                    if(maxLength==-1)maxLength = 0;
                    var length = text.length;
                    if(maxLength != 0)
                    {
                         if(text.length < parseInt(minLength) ||  text.length > parseInt(maxLength)){
                            $.messager.alert('错误',title + '字符长度不对,请核实','error',function(){
                                    element.focus();
                            });
                            isSubmit = false;
                            return false;
                        }
                    }
                    else{
                        if(text.length < parseInt(minLength)){
                            $.messager.alert('错误',title + '字符长度不对,请核实','error',function(){
                                    element.focus();
                            });
                            isSubmit = false;
                            return false;
                        }
                    }
                    
            break;
            case 'orgName':
                    var minLength =  element.attr('minLength');
                    var maxLength =  element.attr('maxLength');
                    if(minLength==undefined)minLength = 0;
                    if(maxLength==undefined)maxLength = 60;
                    if(maxLength==-1)maxLength = 60;
                    var length = text.length;
                     if(length < parseInt(minLength) || length > parseInt(maxLength)){
                        $.messager.alert('错误',title + '字符长度不对,请核实','error',function(){
                                element.focus();
                        });
                        isSubmit = false;
                        return false;
                    }
                    var reg = new RegExp('^[^\x00-\x1F\x5F][^\x00-\x1F]{0,' + maxLength + '}$');
                    if(!reg.test(text)){
                       $.messager.alert('错误',title + '包含有非法字符,请核实','error',function(){
                                element.focus();
                        });
                       isSubmit = false;
                       return false;
                    }
            break;
            case 'userName':
                    var minLength =  element.attr('minLength');
                    var maxLength =  element.attr('maxLength');
                    if(minLength==undefined)minLength = 0;
                    if(maxLength==undefined)maxLength = 60;
                    if(maxLength==-1)maxLength = 60;
                    var length = text.length;
                     if(length < parseInt(minLength) || length > parseInt(maxLength)){
                       $.messager.alert('错误',title + '字符长度不对,请核实','error',function(){
                                element.focus();
                        });
                       isSubmit = false;
                       return false;
                    }
                    //var reg = new RegExp('^[\x20\x2e\x30-\x39\x41-\x5a\x61-\x7a][\x20\x2e\x30-\x39\x41-\x5a\x5f\x61-\x7a]{0,' + maxLength + '}$');
                    var reg = new RegExp('^[^\\x00-\\x2c\\x2f\\x3a-\\x3c\\x3e\\x40\\x5b-\\x5d\\x7b\\x7d]{2,' + maxLength + '}$');
                    if(!reg.test(text)){
                       $.messager.alert('错误',title + '不能包含非法字符','error',function(){
                                element.focus();
                        });
                       isSubmit = false;
                       return false;
                    }
            break;
       }
    });
    return isSubmit;
}