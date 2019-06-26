import { recieveUsersData } from "../actions/types";

const initialState = {
  users: []
};

export default (state = initialState, action = {}) => {
  state = state || initialState;
  if (action.type === recieveUsersData) {
    return {
      ...state,
      users: action.users
    };
  }
  return state;
}