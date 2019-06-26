import axios from 'axios';
import { recieveUsersData } from './types';

export function pickDate(data) {
    return dispatch => {
        return axios.post('https://localhost:44326/api/Account/pickstudydate', data);
    }
};

export function getUsers(search = null,selectedsort = null) {
    let optionalUrl = '';
    console.log(search);
    if (search!=null && search !== '')
    {
        optionalUrl += '&name=' + search;
    }
    console.log(selectedsort);

    if (selectedsort!=null)
    {
        optionalUrl += '&sortOrder=' + selectedsort;
    }
    return dispatch => {
        return axios.get('https://localhost:44326/api/Admin/all?page=1'+optionalUrl).then(
            res => { var users = res.data.users; 
                dispatch({type : recieveUsersData, users});
                
            });
    }
}
export function studyInfo(data) {
    return dispatch => {
        return axios.get('https://localhost:44326/api/Account/studyinfo?id=' + data.id);
    }
};