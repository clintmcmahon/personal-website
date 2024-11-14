---
title: "How To Create A Reusable Select List In React"
description: ""
date: "2020-07-19"
draft: false
slug: "how-to-create-a-reusable-select-list-in-react-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<p>This tutorial is on how to create a reusable dropdown list component in React. I broke the functionality down into two components — the actual select dropdown list component and the calling parent/calling component.</p>
<p>The parent component will be App.js and a component called DynamicSelect.js will handle the select list functionality. The entire source code project is on <a href="https://github.com/clintmcmahon/react-select-dropdown-example">GitHub</a>.</p>
<p>Let’s start with the DynamicSelect component. The DynamicSelect component is the component which will render an array of Seinfeld characters into select list and pass back the selected value via the props object to the parent component. When the onChange event is fired for the select list the event is passed into the handleChange function. This function will pass the selected value back to the parent (App.js) via the props object.</p>
<p><strong>DynamicSelect.js Component</strong></p>
<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace">
<pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">import React, {Component} from 'react';class DynamicSelect extends Component{    constructor(props){        super(props)    }    //On the change event for the select box pass the selected value back to the parent    handleChange = (event) =>    {        let selectedValue = event.target.value;        this.props.onSelectChange(selectedValue);    }    render(){        let arrayOfData = this.props.arrayOfData;        let options = arrayOfData.map((data) =>                &lt;option                     key={data.id}                    value={data.id}                >                    {data.name}                &lt;/option>            );            return (            &lt;select name="customSearch" className="custom-search-select" onChange={this.handleChange}>                &lt;option>Select Item&lt;/option>                {options}           &lt;/select>        )    }}export default DynamicSelect;</pre>
</div>
<p><strong>App.js Component</strong></p>
<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace">
<pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">import React, { Component } from 'react';import logo from './logo.svg';import './App.css';import DynamicSelect from './DynamicSelect';const arrayOfData = [  {    id: '1 - Jerry',    name: 'Jerry'      },  {    id: '2 - Elaine',    name: 'Elaine'      },  {    id: '3 - Kramer',    name: 'Kramer'      },  {    id: '4 - George',    name: 'George'      },];class App extends Component {  constructor(props){    super(props)    this.state={      selectedValue: 'Nothing selected'    }  }  handleSelectChange = (selectedValue) =>{    this.setState({      selectedValue: selectedValue    });  }  render() {    return (      &lt;div className="App">        &lt;header className="App-header">          &lt;img src={logo} className="App-logo" alt="logo" />          &lt;h1 className="App-title">Dynamic Select Dropdown List&lt;/h1>        &lt;/header>        &lt;p className="App-intro">          &lt;DynamicSelect arrayOfData={arrayOfData} onSelectChange={this.handleSelectChange} /> &lt;br />&lt;br />          &lt;div>            Selected value: {this.state.selectedValue}          &lt;/div>        &lt;/p>      &lt;/div>    );  }}export default App;</pre>
</div>
<p>And that&#8217;s it. The repository for this code is available on <a href="https://github.com/clintmcmahon/react-select-dropdown-example">GitHub</a> in this class fashion. This functionatliy can be easily replicated via functional components with hooks as well.</p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
