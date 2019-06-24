import axios from 'axios';

export function pickDate(data) {
    console.log(data);
    return dispatch => {
        return axios.post('https://localhost:44326/api/Account/forgotpassword', data);
    }
};