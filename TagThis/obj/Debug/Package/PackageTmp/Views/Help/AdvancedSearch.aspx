<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	TagThis Advanced Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>What is the Advanced Search?</h2>
<p>The Advanced Search is a tool to help you narrow your results by writting a query, so you can find more accurate results. Bellow are some examples to help you understand how it works.</p>

<div class="instructions">
<h3 style="color: #E5001C;"><strong>Example:</strong> A simple query.</h3>
<p>What if you want to search for "free" and "music" but not "Elvis"? Just drop the query bellow into the Advanced Search Box.</p>
<div class="integrate-url">free + music - Elvis</div>
</div>

<div class="instructions">
<h3 style="color: #E5001C;"><strong>Example:</strong> Another simple one</h3>
<p>TagThis simple search searches by including all terms. But what if you want to search for something like "music" or "tunes" or "rock"? Just drop the query bellow into the Advanced Search Box.</p>
<div class="integrate-url">(music | tunes | rock)</div>
<p>Note: "Or" queries must be between brackets all the time, otherwise the query wont work.</p>
</div>

<div class="instructions">
<h3 style="color: #E5001C;"><strong>Example:</strong> A more complicated one</h3>
<p>What if you want to search for "Music" or "Tunes" and free but not "Elvis"? Just drop the query bellow into the Advanced Search Box.</p>
<div class="integrate-url">(music | tunes)+ free - Elvis</div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
