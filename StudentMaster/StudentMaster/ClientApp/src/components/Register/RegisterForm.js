import React, {
    Component
} from 'react';
import classnames from 'classnames';
import PropTypes from 'prop-types';
import {
    connect
} from "react-redux";
import {
    register,
    social_login
} from "../../actions/authActions";
import {
    Redirect
} from 'react-router-dom';
import FacebookLogin from 'react-facebook-login';
import './RegisterForm.css'
import DatePicker from 'react-date-picker';
import {isValidPassword,isValidEmail} from '../../utils/validators'


class RegisterForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            confirmPassword: '',
            errors: {},
            done: false,
            isLoading: false,
            firstName: '',
            lastName: '',
            birthDate: new Date()
        };
        this.signup = this.signup.bind(this);
    }

    setStateByErrors = (name, value) => {
        if (!!this.state.errors[name]) {
            let errors = Object.assign({}, this.state.errors);
            delete errors[name];
            this.setState({
                [name]: value,
                errors
            })
        } else {
            this.setState({
                [name]: value
            })
        }
    }
    handleDateChange = (e) => {
        if (!!this.state.errors.birthDate) {
            let errors = Object.assign({}, this.state.errors);
            delete errors.birthDate;
            this.setState({
                birthDate: e,
                errors
            })
        } else {
            this.setState({
                birthDate: e
            })
        }
    }
    handleChange = (e) => {

        this.setStateByErrors(e.target.name, e.target.value);
    }

    signup(res, type) {
        if (type === 'facebook') {
            this.props.social_login({
                name: res.name,
                email: res.email
            })
                .then(() => {
                    this.setState({
                        done: true
                    });

                },

                    (err) => {
                        this.setState({
                            isLoading: false
                        });
                    }
                );
        };
    }

    onSubmitForm = (e) => {
        e.preventDefault();

        let errors = {};
        if (isValidEmail(this.state.email)) errors.email = "Enter valid email"
        if (this.state.email === '') errors.email = "Can't be empty"

        if (isValidPassword(this.state.password)) errors.password = "Password must be at least 6 characters and contain digits, upper or lower case"
        if (this.state.password === '') errors.password = "Can't be empty"
        if (this.state.confirmPassword === '') errors.confirmPassword = "Can't be empty"


        if (this.state.confirmPassword !== this.state.password) errors.confirmPassword = "Passwords do not match"

        const isValid = Object.keys(errors).length === 0
        if (isValid) {
            const {
                email,
                password,
                confirmPassword,
                firstName,
                lastName,
                birthDate
            } = this.state;
            this.setState({
                isLoading: true
            });
            this.props.register({
                email,
                password,
                confirmPassword,
                firstName,
                lastName,
                birthDate
            })
                .then(
                    () => this.setState({
                        done: true
                    }),

                    (err) => {
                        this.setState({
                            errors: err.response.data,
                            isLoading: false
                        });
                    }
                );
        } else {
            this.setState({
                errors
            });
        }
    }
    render() {
        const { errors, isLoading } = this.state;
        const clickFacebook = (response) => {
            this.signup(response, 'facebook');
        }
        const form = (
            <div>
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
                    <div className={classnames('form-group', { 'has-error': !!errors.FirstName })}>

                        <input type="text"
                            className="form-control"
                            id="firstName"
                            name="firstName"
                            placeholder="First Name"

                            value={this.state.firstName}
                            onChange={this.handleChange} />
                        {!!errors.FirstName ? <span className="help-block">{errors.FirstName}</span> : ''}
                    </div>
                    <div className={classnames('form-group', { 'has-error': !!errors.LastName })}>
                        <input type="text"
                            className="form-control"
                            id="lastName"
                            name="lastName"
                            placeholder="Last Name"

                            value={this.state.lastName}
                            onChange={this.handleChange} />
                        {!!errors.LastName ? <span className="help-block">{errors.LastName}</span> : ''}
                    </div>
                    <div className={classnames('form-group', { 'has-error': !!errors.BirthDate })}>
                        <DatePicker
                            className="date"
                            onChange={this.handleDateChange}
                            value={this.state.birthDate}
                        />
                        {!!errors.BirthDate ? <span className="help-block">{errors.BirthDate}</span> : ''}
                    </div>
                    <div className="form-group">
                        <button type="submit" className="btn btn-info btn-block" disabled={isLoading}>Register</button>
                    </div>

                </form>
                <FacebookLogin
                    appId="355421778452383"
                    autoLoad={false}
                    cookie={false}
                    xfbml={false}
                    fields="name,email"
                    cssClass="my-facebook-button-class"
                    icon="fa-facebook"

                    callback={clickFacebook} />
            </div>
        );
        return (
            this.state.done ?
                <Redirect to="/confirmemail" /> : form
        );
    }
}

RegisterForm.propTypes = {
    register: PropTypes.func.isRequired,
    social_login: PropTypes.func.isRequired

}

export default connect(null, {
    register,
    social_login
})(RegisterForm);