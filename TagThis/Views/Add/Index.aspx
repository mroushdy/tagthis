<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tag a Webpage</h2>
    Enter page url below<br /><br />
<div style="background-color: #EdEdEd; width:100%; padding:10px 10px 10px 10px;">

<% using (Ajax.BeginForm("Add", "Add", new {}, new AjaxOptions() { UpdateTargetId = "error", InsertionMode = InsertionMode.Replace }, new {}))
   {%>
             <div style="width:400px">
           
             <%= Html.TextBox("url")%>
             <%= Html.TextBox("desc")%>
             <%= Html.TextBox("Tags")%>
              <input type="submit" value="Save" />
             </div>

             <%} %>

              <div class="error" id="error" style="width:400px;">
              </div>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">


<script type="text/javascript" charset="utf-8">


    $(document).ready(function () {
        var t2 = new TextboxList('#Tags', { unique: true, bitsOptions: { editable: { addKeys: 32, addOnBlur: true}} });
    });
</script>


<style type="text/css" media="screen">
			.form_tags { margin-bottom: 10px;}
			
			/* Setting widget width example */
			.form_tags .textboxlist, #form_friends .textboxlist { width: 400px; }
			
			/* Preloader for autocomplete */
			.textboxlist-loading { background: url('../content/close.gif') no-repeat 380px center; }
			
			/* Autocomplete results styling */
			#form_friends .textboxlist-autocomplete-result { overflow: hidden; zoom: 1; }
			#form_friends .textboxlist-autocomplete-result img { float: left; padding-right: 10px; }
			
			.note { color: #666; font-size: 90%; }
			#footer { margin: 50px; text-align: center; }
			
			.content {
            width: 400px ;
            margin-left: auto ;
            margin-right: auto ;
                   }
               .button {
            width: 21px ;
            height: 19px;
            cursor: pointer;
                   }
		</style>

</asp:Content>
