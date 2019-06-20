import React, {Component} from 'react';
import PropTypes from 'prop-types';
import  { login } from  '../../actions/authActions';
import { connect } from 'react-redux';


class LoginForm extends Component {

    constructor(props){
        super(props);
        this.state = {
            email: '',
            password: ''
        }

        this.onChange = this.onChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

    }
    onChange(e){
        this.setState({[e.target.name]: e.target.value});
    }
    onSubmit(e){
        e.preventDefault();
        this.props.login(this.state).then(
            (res) => this.context.router.push('/'));
        console.log(this.state);
    }
  render () {

    
    return (
      <form onSubmit={this.onSubmit}>
        <div className="form-group"> 
        <input
        value={this.state.email}
        className= "form-control"
          name="email"
          component="input"
          type="text"
          onChange={this.onChange}
          placeholder="Email"
        />
        </div>
        <div className="form-group"> 
        <input
        value={this.state.password}
        className= "form-control"
          name="password"
          component="input"
          type="password"
          onChange={this.onChange}
          placeholder="Password"
        />
        </div>

        <button className="btn btn-primary " type="submit" label="submit">Submit</button>
      </form>
    );
  }
}

LoginForm.propTypes = {
    login: PropTypes.func.isRequired
  }
  
  LoginForm.contextTypes = {
    router: PropTypes.object.isRequired
  }
  

export default connect(null, { login })(LoginForm);