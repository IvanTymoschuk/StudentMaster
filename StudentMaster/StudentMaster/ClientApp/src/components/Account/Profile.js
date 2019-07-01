import React from 'react';
import PropTypes, {
} from 'prop-types';
import {
    connect
} from "react-redux";
import {
    getUserById
} from "../../actions/accActions";

class Profile extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            firstName: '',
            lastName: '',
            email: ''
        }

    }
    componentDidMount() {
        const id = this.props.auth.user.id;
        this.props.getUserById(id)
            .then(res => {
                this.setState(res.data);

            });
    }
    render() {

        return (
            <div>
                <h1>{this.props.auth.user.roles} profile</h1>
                <table className="table">
                    <tbody>
                        <tr>
                            <th>First Name</th>
                            <td>{this.state.firstName}</td>
                        </tr>
                        <tr>
                            <th>Last Name</th>
                            <td>{this.state.lastName}</td>
                        </tr>
                        <tr>
                            <th>Email</th>
                            <td>{this.state.email}</td>
                        </tr>
                    </tbody>
                </table>
            </div>)
    }
}
Profile.propTypes = {
    getUserById: PropTypes.func.isRequired
}
const mapStateToProps = (state) => {
    return {
        auth: state.auth
    };
}

export default connect(mapStateToProps, {
    getUserById
})(Profile);