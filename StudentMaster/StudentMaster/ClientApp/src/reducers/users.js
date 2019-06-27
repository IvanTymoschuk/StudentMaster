import { recieveUsersData, getAllPages } from "../actions/types";

const initialState = {
  users: [],
  pages: 0
};

export default (state = initialState, action = {}) => {
  state = state || initialState;
  if (action.type === recieveUsersData) {
    return {
      ...state,
      users: action.users
    };
  }

if (action.type === getAllPages) {
  return {
    ...state,
    pages: action.pages
    }
  };
  return state;
}