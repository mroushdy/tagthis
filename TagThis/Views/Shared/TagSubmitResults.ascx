<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

                       <div>
                          <table border="0" cellspacing="0" cellpadding="0">
  <tr>
  <td>
			        <%if (ViewData["Messages"] != null)
                    {
                        foreach (string Message in (ViewData["Messages"] as List<string>))
                        { %>
                        <%=Message%>
                        <%}}else{%>
                        No changes have been made.
                        <%}%>
                        
                        <%if(ViewData["nothinghappened"].ToString() == "yes"){ %>
                        No changes have been made.
                        <%}%>
                        </div>
                        </td>
                        <td valign="bottom">
                        <div id="CancellButton" class="button" onclick="toggleLayer('tagbox<%= Html.Encode(ViewData["pageid"])%>');javascript:toggleLayer('tagthisbutton<%= Html.Encode(ViewData["pageid"])%>');" style="background:transparent url(../../Content/icons/delete16.gif) no-repeat;" />
                    </td>
                    </tr>
                    </table>