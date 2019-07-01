import React from 'react';
import PropTypes, {
} from 'prop-types';
import {
    connect
} from "react-redux";
import {
    getUserById,
    editUser
} from "../../actions/accActions";
import classnames from 'classnames';
import './EditUser.css';

class EditUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            userId: '',
            firstName: '',
            lastName: '',
            email: '',
            errors: {},
        }

    }
    componentDidMount() {
        const id = this.props.id;
        this.setState({ userId: id });

        this.props.getUserById(id)
            .then(res => {

                this.setState(res.data);

            });


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

    onSubmitForm = (e) => {
        e.preventDefault();

        let errors = {};
        if (this.isValidEmail(this.state.email)) errors.email = "Enter valid email"
        if (this.state.email === '') errors.email = "Can't be empty"

        const isValid = Object.keys(errors).length === 0

        if (isValid) {
            const {
                userId,
                email,
                firstName,
                lastName
            } = this.state;
            this.setState({
                isLoading: true
            });
            this.props.editUser({
                userId,
                email,
                firstName,
                lastName
            })
                .then(
                    () => {
                        this.setState({
                            done: true
                        })
                        this.props.onClick()
                    },

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
    handleChange = (e) => {

        this.setStateByErrors(e.target.name, e.target.value);
    }
    isValidEmail(email) {

        return !/.+@.+\.[A-Za-z]+$/.test(email);
    }


    render() {
        const { errors } = this.state;
        return (
            <div>

                <form onSubmit={this.onSubmitForm}>
                    <table className="table">
                        <tbody>
                            <tr>
                                <th>First Name</th>
                                <td><div className={classnames('form-group margins', { 'has-error': !!errors.FirstName })}>

                                    <input type="text"
                                        className="form-control"
                                        id="firstName"
                                        name="firstName"
                                        placeholder="First Name"

                                        value={this.state.firstName}
                                        onChange={this.handleChange} />
                                    {!!errors.FirstName ? <span className="help-block">{errors.FirstName}</span> : ''}
                                </div></td>
                            </tr>
                            <tr>
                                <th>Last Name</th>
                                <td><div className={classnames('form-group margins', { 'has-error': !!errors.LastName })}>
                                    <input type="text"
                                        className="form-control"
                                        id="lastName"
                                        name="lastName"
                                        placeholder="Last Name"

                                        value={this.state.lastName}
                                        onChange={this.handleChange} />
                                    {!!errors.LastName ? <span className="help-block">{errors.LastName}</span> : ''}
                                </div></td>
                            </tr>
                            <tr>
                                <th>Email</th>
                                <td><div className={classnames('form-group margins', { 'has-error': !!errors.email })}>
                                    <input type="text"
                                        className="form-control"
                                        id="email"
                                        name="email"
                                        placeholder="Email"
                                        value={this.state.email}
                                        onChange={this.handleChange} />
                                    {!!errors.email ? <span className="help-block">{errors.email}</span> : ''}
                                </div></td>
                            </tr>

                        </tbody>
                    </table>
                    <div className="form-group">


                        <button type="submit" className="btn btn-info" >Edit</button>


                    </div>
                </form>
                <button onClick={this.props.onClick} className="btn btn-warn" >close</button>

            </div>)
    }
}
EditUser.propTypes = {
    getUserById: PropTypes.func.isRequired
}
const mapStateToProps = (state) => {
    return {
        auth: state.auth
    };
}

export default connect(mapStateToProps, {
    getUserById, editUser
})(EditUser);