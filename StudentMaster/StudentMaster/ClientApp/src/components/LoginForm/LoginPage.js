import React from 'react';
import LoginForm from './LoginForm';



class LoginPage extends React.Component {
    render() {
        const {userSignUpRequest} = this.props;
      return (
        <div className="row">
          <div className="col-md-4 col-md-offset-4">
            <LoginForm />
          </div>
        </div>
      );
    }
  }
  

  export default LoginPage;