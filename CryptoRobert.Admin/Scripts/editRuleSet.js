function removeItem() {
	var tr = document.getElementById("");
	var candidate = document.getElementById("@rule.RuleDef.Id");
	var item = document.getElementById(candidate.value);
	ul.removeChild(item);
}