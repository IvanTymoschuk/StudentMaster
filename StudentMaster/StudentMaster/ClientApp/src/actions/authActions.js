import axios from 'axios';
import setAuthorizationToken from '../utils/setAuthorizationToken';
import jwt from 'jsonwebtoken';
import jwtDecode from 'jwt-decode';
import { SET_CURRENT_USER } from './types';

export function setCurrentUser(user) {
    return {
        type: SET_CURRENT_USER,
        user
    };
}

export function logout() {
    return dispatch => {
        localStorage.removeItem('jwtToken');
        setAuthorizationToken(false);
        dispatch(setCurrentUser({}));
    }
}
export function forgotPassword(data) {
    return dispatch => {
        return axios.post('https://localhost:44326/api/Account/forgotpassword', data);
    }
};

export function resetPassword(data) {
    return dispatch => {
        return axios.post('https://localhost:44326/api/Account/resetpassword', data);
    }
};
export function register(data) {

    return dispatch => {
        return axios.post('https://localhost:44326/api/Registration/registration', data).then(res => {
            this.props.history.push('/confirmemail');
        });

    }
}
export function social_login(data) {

    return dispatch => {
        return axios.post('https://localhost:44326/api/Account/sociallogin', data).then(res => {
            loginByJWT(res.data.token, dispatch);
        });
    }
}
const loginByJWT = (token, dispatch) => {

    localStorage.setItem('jwtToken', token);
    setAuthorizationToken(token);
    dispatch(setCurrentUser(jwtDecode(token)));
}
export function login(data) {

    return dispatch => {
        return axios.post('https://localhost:44326/api/Auth/login', data).then(res => {
            loginByJWT(res.data.token, dispatch);
        });
    }
}