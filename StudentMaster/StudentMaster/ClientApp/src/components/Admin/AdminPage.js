import React, { Component } from 'react';
import { getUsers } from "../../actions/accActions";
import PropTypes from 'prop-types';
import { connect } from "react-redux";
import Select from 'react-select';
import Moment from 'react-moment';
const sorting = [
    { label: "First Name Up", value: 0 },
    { label: "First Name Down", value: 1 },
    { label: "Last Name Up", value: 2 },
    { label: "Last Name Down", value: 3 },
    { label: "Age Up", value: 4 },
    { label: "Age Down", value: 5 },
    { label: "Registred Date Up", value: 6 },
    { label: "Registred Date Down", value: 7 },
    { label: "Study Date Up", value: 8 },
    { label: "Study Date Down", value: 9 },
];

class AdminPage extends Component {
    constructor(props) {
        super(props)
        this.state = {
            search: '',
            selectedsort: sorting[0],
            sortasc: true

        }
        this.handleChange = this.handleChange.bind(this);
        this.handleClickEdit = this.handleClickEdit.bind(this);
    }
    componentWillMount() {

        this.props.getUsers();
    }
    componentDidMount() {

    }
    handleChange = (e) => {
        this.setState(
            { search: e.target.value }, () =>
                this.props.getUsers(this.state.search, this.stateselectedsort)
        )

    }
    handleClickEdit(id) {
        console.log(id)
    }
    handleSort(column) {
        if (this.state.selectedsort.value == column)
            column++;
            
        this.setState(
            { selectedsort:  sorting[column]}, () =>
            this.props.getUsers(this.state.search, this.state.selectedsort.value))

    }
    selectChanged = (e) => {
        this.setState(
            { selectedsort: sorting[e.value] }, () =>

                this.props.getUsers(this.state.search, this.state.selectedsort.value)
        )
    }
    render() {

        return (
            <div>

                <div className="input-group input-group-lg">

                    <input placeholder="Search" value={this.state.search} type="text" className="form-control" onChange={this.handleChange} aria-label="Sizing example input" aria-Downribedby="inputGroup-sizing-lg" />
                    <span class="input-group-btn ">

                    </span>


                </div>
                <Select 
                    options={sorting} 
                    value={this.state.selectedsort} 
                    isClearable={true}
                    defaultValue={0} 
                    onChange={this.selectChanged} />
                <table className="table table-hover">
                    <thead className="thead-dark">
                        <tr>
                            <th onClick={() => this.handleSort()}>Id</th>
                            <th onClick={() => this.handleSort(0)}>First Name</th>
                            <th onClick={() => this.handleSort(2)}>Last Name</th>
                            <th onClick={() => this.handleSort(4)}>Age</th>
                            <th>Email</th>
                            <th onClick={() => this.handleSort(6)}>Registration Date</th>
                            <th onClick={() => this.handleSort(8)}>Study Date</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.users.map(ct =>
                            <tr>
                                <th scope="row">{ct.userId}</th>
                                <td>{ct.firstName}</td>
                                <td>{ct.lastName}</td>
                                <td>{ct.age}</td>
                                <td>{ct.email}</td>
                                <td><Moment format="YYYY/MM/DD">{ct.registredDate}</Moment></td>
                                <td><Moment format="YYYY/MM/DD">{ct.studyDate}</Moment></td>
                                <td> <button className="btn" onClick={() => this.handleClickEdit(ct.userId)}>Edit</button></td>
                            </tr>

                        )}
                    </tbody>
                </table>
            </div>

        )
    }
}

AdminPage.propTypes =
    {
        getUsers: PropTypes.func.isRequired,

    }
const mapStateToProps = (state) => {
    return {
        users: state.users.users
    };
}

export default connect(mapStateToProps, { getUsers })(AdminPage);