//<![CDATA[
//-- check variables
//-- Check if a value exists
function isEmpty(obj)
{
	var isVal = false;
	try {
		if (obj.length < 1){
			isVal = true;
		}
		return isVal;
	}catch(e){
		alert(e.description);
	}
}
function isVar()
{
	var isBe = false;
	if(typeof(arguments[0]) != 'undefined'){
		isBe = true;
	}
	return isBe;
}
//showhide object
function showhideObj()
{
	//showhideObj('prefix string of target for id', target index, total length, 'object for call', 'classname for change', 'classname for rollback')
	//showhideObj('showhide target id');
	var isClassName = true;
	var objCntStr = null;
	var objBtnStr = "";
	var isTab = false;
	var trgObj = null;
	try {
		objCntStr = arguments[0];
		trgObj = document.getElementById(objCntStr);
		
		if(arguments.length > 1) {
			trgObj = document.getElementById(objCntStr+arguments[1]);
		}
		if (arguments[3] != undefined) {
			isTab = true;
			var isTabObj = null;
			objBtnStr = arguments[3];
		}
		
		if (isEmpty(trgObj.className)) {//값 없음, style로 지정
			isClassName = false;
		}
		if (arguments[2] != undefined && arguments[2] > 1) {
			var isObj = null;
			for (var i=1; i<=arguments[2]; i++ ) {
				isObj = document.getElementById(objCntStr+i);
				if(isTab) {
					isTabObj = document.getElementById(objBtnStr+i);
				}
				if (isVar(isObj)) {					
					if (trgObj == isObj) {						
						if (isClassName) {
							isObj.className = "areashow";
						} else {
							isObj.style.display = "block";
						}
						if(isTab) isTabObj.className = arguments[4];
					} else {
						if (isClassName) {
							isObj.className = "areahide";
						} else {
							isObj.style.display = "none";
						}
						if(isTab) isTabObj.className = arguments[5];
					}
				}
			}
		} else {			
			if (isClassName) {
				if (trgObj.className == "areahide") {
					trgObj.className = "areashow";
				} else {
					trgObj.className = "areahide";
				}
			} else {
				if (trgObj.style.display == "none") {
					trgObj.style.display = "block";
				} else {
					trgObj.style.display = "none";
				}
			}
		}
	} catch(e) {
//		alert(e.description);
	}	
}

//]]>