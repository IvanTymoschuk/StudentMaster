import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import LoginForm from './components/LoginForm/LoginForm';



export default () => (
  
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/counter' component={Counter} />
    <Route path='/login' component={LoginForm} />

    

  </Layout>
);
