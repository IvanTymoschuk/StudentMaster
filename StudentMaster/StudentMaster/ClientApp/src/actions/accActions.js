import axios from 'axios';

export function pickDate(data) {
    console.log(data);
    return dispatch => {
        return axios.post('https://localhost:44326/api/Account/pickstudydate', data);
    }
};

export function studyInfo(data) {
    console.log(data);
    return dispatch => {
        return axios.get('https://localhost:44326/api/Account/studyinfo?id='+data.id);
    }
};