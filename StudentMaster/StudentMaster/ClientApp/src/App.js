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
import requireAuth from './utils/requireAuth';
import PickDatePage from './components/Account/PickDatePage';
import StudyInfo from './components/Account/StudyInfo';
import Profile from './components/Account/Profile';







export default () => (

  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/login' component={LoginPage} />
    <Route path='/registration' component={RegisterPage} />
    <Route path="/forgotpassword" component={ForgotPasswordPage} />
    <Route path="/resetpassword" component={ResetPasswordPage} />
    <Route path="/confirmemail" component={ConfirmEmail} />
    <Route path="/pickdate" component={requireAuth(PickDatePage, "user")} />
    <Route path="/schedule" component={requireAuth(StudyInfo, "user")} />
    <Route path="/profile" component={Profile} />



    <Route path='/admin' component={requireAuth(AdminPage, "admin")} />

  </Layout>
);
