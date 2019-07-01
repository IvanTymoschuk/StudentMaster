import React, {
    Component
} from 'react';
import {
    getUsers
} from "../../actions/accActions";
import PropTypes from 'prop-types';
import {
    connect
} from "react-redux";
import Select from 'react-select';
import Moment from 'react-moment';
import {
    Pagination,
    Col
} from "react-bootstrap";
import "./AdminPage.css"
import EditUser from '../Account/EditUser';
const sorting = [{
        label: "First Name Up",
        value: 0
    },
    {
        label: "First Name Down",
        value: 1
    },
    {
        label: "Last Name Up",
        value: 2
    },
    {
        label: "Last Name Down",
        value: 3
    },
    {
        label: "Age Up",
        value: 4
    },
    {
        label: "Age Down",
        value: 5
    },
    {
        label: "Registred Date Up",
        value: 6
    },
    {
        label: "Registred Date Down",
        value: 7
    },
    {
        label: "Study Date Up",
        value: 8
    },
    {
        label: "Study Date Down",
        value: 9
    },
];

class AdminPage extends Component {
    constructor(props) {
        super(props)
        this.state = {
            editUserId: '',
            search: '',
            selectedsort: sorting[0],
            sortasc: true,
            currentPage: 1


        }
        this.handleChange = this.handleChange.bind(this);
        this.handleClickEdit = this.handleClickEdit.bind(this);
        this.onClickId = this.onClickId.bind(this);

    }
    componentWillMount() {
        this.props.getUsers();
    }
    componentDidMount() {

    }
    handleChange = (e) => {
        this.setState({
                currentPage: 1,
                search: e.target.value
            }, () =>
            this.props.getUsers(this.state.currentPage, this.state.search, this.stateselectedsort)
        )

    }
    handleClickEdit(id) {

        this.setState({editUserId:id});
        
        
        
    }
    handleSort(column) {
        if (this.state.selectedsort.value === column)
            column++;

        this.setState({
                selectedsort: sorting[column]
            }, () =>
            this.props.getUsers(this.state.currentPage, this.state.search, this.state.selectedsort.value))

    }
    selectChanged = (e) => {
        this.setState({
                selectedsort: sorting[e.value]
            }, () =>

            this.props.getUsers(this.state.currentPage, this.state.search, this.state.selectedsort.value)
        )
    }
    handlePageChange(pageNumber) {
        this.setState({
                currentPage: pageNumber
            }, () =>
            this.props.getUsers(this.state.currentPage, this.state.search, this.state.selectedsort.value)
        );
    }

    onClickId(){

        this.setState({editUserId: null});
    }
    render() {

        return (
            <div>
                <div className="row align-items-center">
                <Col md={6}><div className="input-group input-group-lg">

<input placeholder="Search" value={this.state.search} type="text" className="search form-control" onChange={this.handleChange} aria-label="Sizing example input"  />
<span className="input-group-btn ">

</span>


</div></Col>
                <Col md={6}><Select
                    options={sorting}
                    value={this.state.selectedsort}
                    isClearable={true}
                    defaultValue={0}
                    onChange={this.selectChanged} />
                    
                    </Col>
                
                </div>
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
                        {this.props.users.map((ct,i) =>
                            <tr key={i}>
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
                
                <Pagination
                className="users-pagination pull-right"
                bsSize="large"
                    maxButtons={this.props.pages}
                    first last next prev
                    items={this.props.pages}
                    onSelect={this.handlePageChange.bind(this)}
                    activePage={this.state.currentPage}
                ></Pagination>
                {this.state.editUserId?<EditUser onClick={this.onClickId} id={this.state.editUserId}></EditUser>:''}
                
            </div>

        )
    }
}

AdminPage.propTypes = {
    getUsers: PropTypes.func.isRequired,


}
const mapStateToProps = (state) => {
    return {
        users: state.users.users,
        pages: state.users.pages

    };
}

export default connect(mapStateToProps, {
    getUsers
})(AdminPage);