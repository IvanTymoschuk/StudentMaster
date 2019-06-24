import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import LoginPage from './components/Login/LoginPage';
import RegisterPage from './components/Register/RegisterPage'
import ForgotPasswordPage from './components/ForgotPasswordPage';
import ResetPasswordPage from './components/ResetPasswordPage';
import ConfirmEmail from './components/Confirmation/ConfirmEmailPage';
 import AdminPage from './components/Admin/AdminPage';
 import requireAuth from "./utils/requireAuth";






export default () => (
  
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/login' component={LoginPage} />
    <Route path='/registration' component={RegisterPage} />
    <Route path="/forgotpassword" component={ForgotPasswordPage} />
    <Route path="/resetpassword" component={ResetPasswordPage} />
    <Route path="/confirmemail" component={ConfirmEmail} />
    <Route path='/admin' component={requireAuth(AdminPage,"admin")} /> 

  </Layout>
);
