import React, { Component } from 'react';
import classnames from 'classnames';
import PropTypes from 'prop-types';
import { connect } from "react-redux";
import { register } from "../../actions/authActions";
import { Redirect } from 'react-router-dom';
import { Row } from 'react-bootstrap'


//const defaultPath = "D:/Study/React/WebBlog/ClientApp//src//components/auth/register/default-user.png"
class RegisterForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            confirmPassword: '',
            errors: {
            },
            done: false,
            isLoading: false,
            firstName: '',
            lastName: '',
            dateOfBirth: ''
        };
    }



    setStateByErrors = (name, value) => {
        if (!!this.state.errors[name]) {
            let errors = Object.assign({}, this.state.errors);
            delete errors[name];
            this.setState(
                {
                    [name]: value,
                    errors
                }
            )
        }
        else {
            this.setState(
                { [name]: value })
        }
    }
    handleChange = (e) => {
        this.setStateByErrors(e.target.name, e.target.value);
    }
    isValidPassword(email) {

        return !/^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$/.test(email);
    }
    isValidEmail(email) {

        return !/.+@.+\.[A-Za-z]+$/.test(email);
    }
    onSubmitForm = (e) => {
        e.preventDefault();

        let errors = {};
        if (this.isValidEmail(this.state.email)) errors.email = "Enter valid email"
        if (this.state.email === '') errors.email = "Can't be empty"

        if (this.isValidPassword(this.state.password)) errors.password = "Password must be at least 6 characters and contain digits, upper or lower case"
        if (this.state.password === '') errors.password = "Can't be empty"
        if (this.state.confirmPassword === '') errors.confirmPassword = "Can't be empty"


        if (this.state.confirmPassword !== this.state.password) errors.confirmPassword = "Passwords do not match"

        const isValid = Object.keys(errors).length === 0
        if (isValid) {
            const { email, password, confirmPassword,
                firstName, lastName, dateOfBirth} = this.state;
            this.setState({ isLoading: true });
            this.props.register({ email, password, confirmPassword,
                firstName, lastName, dateOfBirth})
                .then(
                    () =>this.setState({ done: true } ),
                    
                    (err) => {
                        this.setState({ errors: err.response.data, isLoading: false });
                    }
                );
        }
        else {
            this.setState({ errors });
        }
      
    }
    render() {
        const { errors, isLoading } = this.state;
        const form = (
            <form onSubmit={this.onSubmitForm}>
                <h2 className="header text-center">Registration</h2>
                {
                    !!errors.invalid ?
                        <div className="alert alert-danger">
                            <strong>Danger!</strong> {errors.invalid}.
                    </div> : ''}

                <div className={classnames('form-group', { 'has-error': !!errors.email })}>
                    <input type="text"
                        className="form-control"
                        id="email"
                        name="email"
                        placeholder="Email"
                        value={this.state.email}
                        onChange={this.handleChange} />
                    {!!errors.email ? <span className="help-block">{errors.email}</span> : ''}
                </div>

                <div className={classnames('form-group', { 'has-error': !!errors.password })}>
                   
                    <input type="password"
                        className="form-control"
                        id="password"
                        name="password"
                        placeholder="Password"
                        value={this.state.password}
                        onChange={this.handleChange} />
                    {!!errors.password ? <span className="help-block">{errors.password}</span> : ''}
                </div>

                <div className={classnames('form-group', { 'has-error': !!errors.confirmPassword })}>
                   
                    <input type="password"
                        className="form-control"
                        id="confirmPassword"
                        name="confirmPassword"
                        placeholder="Confirm password"
                        value={this.state.confirmPassword}
                        onChange={this.handleChange} />
                    {!!errors.confirmPassword ? <span className="help-block">{errors.confirmPassword}</span> : ''}
                </div>
                <div className={classnames('form-group', { 'has-error': !!errors.firstName })}>
                    
                    <input type="text"
                        className="form-control"
                        id="firstName"
                        name="firstName"
                        placeholder="First Name"

                        value={this.state.firstName}
                        onChange={this.handleChange} />
                    {!!errors.firstName ? <span className="help-block">{errors.firstName}</span> : ''}
                </div>
                <div className={classnames('form-group', { 'has-error': !!errors.lastName })}>
                    <input type="text"
                        className="form-control"
                        id="lastName"
                        name="lastName"
                        placeholder="Last Name"

                        value={this.state.lastName}
                        onChange={this.handleChange} />
                    {!!errors.lastName ? <span className="help-block">{errors.lastName}</span> : ''}
                </div>
                <div className={classnames('form-group', { 'has-error': !!errors.dateOfBirth })}>
                    <input type="Date"
                        className="form-control"
                        id="dateOfBirth"
                        name="dateOfBirth"
                        placeholder="Date of Birth"

                        value={this.state.dateOfBirth}
                        onChange={this.handleChange} />
                    {!!errors.dateOfBirth ? <span className="help-block">{errors.dateOfBirth}</span> : ''}
                </div>
                <div className="form-group">
               
                   
                    <button type="submit" className="btn btn-info btn-block" disabled={isLoading}>Register</button>
                
                </div>

            </form>
        );
        return (
            this.state.done ?
                <Redirect to="/" /> : form
        );
    }
}
    
RegisterForm.propTypes =
    {
        register: PropTypes.func.isRequired
    }

export default connect(null, { register })(RegisterForm);