import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { login } from '../../actions/authActions';
import { connect } from 'react-redux';
import classnames from 'classnames';


class LoginForm extends Component {

    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            errors: {},
            done: false,
            isLoading: false,
        }

        this.onChange = this.onChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

    }

    onChange(e) {
        this.setStateByErrors(e.target.name, e.target.value);
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
    state = {
        email: '',
        password: '',
        errors: {
        },
        done: false,
        isLoading: false
    }
    isValidPassword(email) {

        return !/^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$/.test(email);
    }
    isValidEmail(email) {

        return !/.+@.+\.[A-Za-z]+$/.test(email);
    }
    onSubmit(e) {
        e.preventDefault();
        let errors = {};
        if (this.state.email === '') errors.email = "Can't be empty!"
        else if (this.isValidEmail(this.state.email)) errors.email = "Email is not valid"
        if (this.state.password === '') errors.password = "Can't be empty!"
        else if (this.isValidPassword(this.state.password)) errors.password = "Password must be at least 6 characters and contain digits, upper or lower case"

        const isValid = Object.keys(errors).length === 0
        if (isValid) {
            const { email, password } = this.state;
            this.setState({ isLoading: true });
            this.props.login(this.state).then(

                () =>{ this.setState({ done: true }),
                    this.context.router.history.push('/');},
            (err) => this.setState({ errors: err.response.data, isLoading: false }))
        }
        else {
            this.setState({ errors });
        }
    }
    render() {

        const { errors, isLoading } = this.state;
        return (
            <form onSubmit={this.onSubmit}>
                <div className="text-center mb-4">
                    <h2 className="header">Log in</h2>
                </div>
                {!!errors.invalid ?
                    <div className="alert alert-danger">
                        {errors.invalid}.
                </div> : ''}
                {errors.form && <div className="alert alert-danger">{errors.form}</div>}
                <div className={classnames('form-group', { 'has-error': !!errors.email })}>
                    <input
                        value={this.state.email}
                        className="form-control"
                        name="email"
                        component="input"
                        type="text"
                        error={errors.email}
                        onChange={this.onChange}
                        placeholder="Email"
                    />
                    {!!errors.email ? <span className="help-block">{errors.email}</span> : ''}

                </div>

                <div className={classnames('form-group', { 'has-error': !!errors.password })}>
                    <input
                        value={this.state.password}
                        className="form-control"
                        name="password"
                        component="input"
                        type="password"
                        error={errors.password}
                        onChange={this.onChange}
                        placeholder="Password"

                    />
                    {!!errors.password ? <span className="help-block">{errors.password}</span> : ''}
                </div>
                <button className="btn btn-info btn-block " type="submit" >Log in</button>


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