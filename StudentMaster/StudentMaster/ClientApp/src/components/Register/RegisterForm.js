import React, { Component } from 'react';
import classnames from 'classnames';
import PropTypes from 'prop-types';
import { connect } from "react-redux";
import { register, social_login } from "../../actions/authActions";
import { Redirect } from 'react-router-dom';
import GoogleLogin from 'react-google-login';
import FacebookLogin from 'react-facebook-login';
import { Row } from 'react-bootstrap'



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
            birthDate: ''
        };
        this.signup = this.signup.bind(this);
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
    signup(res, type) {
        if (type === 'facebook') {
            this.props.social_login({
                name: res.name,
                email: res.email
            })
                .then(() => {this.setState({ done: true }) ; this.context.router.history.push('/') },

                    (err) => {
                        this.setState({ errors: err.response.data, isLoading: false });
                    }
                );
        };


        // if (type === 'google') {
        //     this.props.social_login({
        //         name: res.w3.ig,
        //         email: res.w3.U3
        //     })
        //         .then(() => { this.context.router.history.push('/'); this.setState({ done: true }) },

        //             (err) => {
        //                 this.setState({ errors: err.response.data, isLoading: false });
        //             }
        //         );
        // };
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
                firstName, lastName, birthDate } = this.state;
            this.setState({ isLoading: true });
            this.props.register({
                email, password, confirmPassword,
                firstName, lastName, birthDate
            })
                .then(
                    () => this.setState({ done: true }),

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
        const responseGoogle = (response) => {
            console.log("google");

            console.log(response);
            this.signup(response, 'google');
        }
        const responseFacebook = (response) => {
            console.log(response);
            this.signup(response, 'facebook');
        }
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
                <div className={classnames('form-group', { 'has-error': !!errors.birthDate })}>
                    <input type="Date"
                        className="form-control"
                        id="birthDate"
                        name="birthDate"
                        placeholder="Date of Birth"

                        value={this.state.birthDate}
                        onChange={this.handleChange} />
                    {!!errors.birthDate ? <span className="help-block">{errors.birthDate}</span> : ''}
                </div>
                <div className="form-group">


                    <button type="submit" className="btn btn-info btn-block" disabled={isLoading}>Register</button>

                </div>
                <div>
                    <GoogleLogin
                        clientId="465803180827-n8oa659teb415347p0a3b2qoq9ir0gvg.apps.googleusercontent.com"
                        buttonText="Login with google"
                        onSuccess={responseGoogle}
                        onFailure={responseGoogle}
                        cookiePolicy={'single_host_origin'}
                    />
                    <FacebookLogin
                        appId="355421778452383"
                        autoLoad={true}
                        fields="name,email"
                        // onClick={componentClicked}
                        callback={responseFacebook} />
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
        register: PropTypes.func.isRequired,
        social_login: PropTypes.func.isRequired

    }

export default connect(null, { register ,social_login})(RegisterForm);