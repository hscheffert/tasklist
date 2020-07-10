import * as React from 'react';
import { Menu, Layout } from 'antd';
import { NavLink } from 'react-router-dom';
import HistoryUtil from 'utils/HistoryUtil';
import Routes from 'config/ConfigureRoutes';
import { UserState } from 'redux/UserReducer';
import ReduxStoreModel from 'redux/ReduxModel';
import { connect } from 'react-redux';

const { Sider } = Layout;

interface SideNavProps {
    User: UserState;
}

interface SideNavState {
    current: string;
}

class SideNav extends React.Component<SideNavProps, SideNavState> {
    private homeUrl: string;
    private areasUrl: string;
    private frequenciesUrl: string;
    private staffTypesUrl: string;
    private usersUrl: string;

    constructor(props: SideNavProps) {
        super(props);
        this.homeUrl = Routes.GET.BASE_ROUTE;
        this.areasUrl = Routes.GET.AREA_BASE;
        this.frequenciesUrl = Routes.GET.FREQUENCY_BASE;
        this.staffTypesUrl = Routes.GET.STAFF_TYPE_BASE;
        this.usersUrl = Routes.GET.USER_BASE;

        this.state = { current: this.getSelectedNavItem(HistoryUtil.location.pathname) };

        HistoryUtil.listen((location) => {
            this.setState({
                current: this.getSelectedNavItem(location.pathname)
            });
        });
    }

    handleClick = (e: any) => {
        this.setState({
            current: e.key,
        });
    }

    renderAdminOnlyMenuItems = () => {
        const items = [
            { key: 'areas', url: this.areasUrl, name: 'Areas' },
            { key: 'frequencies', url: this.frequenciesUrl, name: 'Frequencies' },
            { key: 'staffTypes', url: this.staffTypesUrl, name: 'Staff Types' },
            { key: 'users', url: this.usersUrl, name: 'Users' }
        ];
        
        return items.map(item => (
            <Menu.Item key={item.key}>
                <NavLink to={item.url}>
                    {item.name}
                </NavLink>
            </Menu.Item>
        ));
    }

    render() {
        const isAdmin = this.props.User.isAdmin;
        const selectedKeys = [this.state.current];        

        return (
            <Sider width={200} className="sider">
                <Menu
                    onClick={this.handleClick}
                    mode="inline"
                    defaultSelectedKeys={['home']}
                    selectedKeys={selectedKeys}
                    style={{ height: '100%', borderRight: 0 }}>
                    <Menu.Item key="home">
                        <NavLink to={this.homeUrl}>
                            Tasks
                        </NavLink>
                    </Menu.Item>
                    {isAdmin && this.renderAdminOnlyMenuItems()}
                </Menu>
            </Sider>
        );
    }

    private getSelectedNavItem(location: string): string {
        const initialLocation = location;
        let selectedItem = '';

        if (initialLocation.indexOf(this.areasUrl) >= 0) {
            selectedItem = 'areas';
        } else if (initialLocation.indexOf(this.frequenciesUrl) >= 0) {
            selectedItem = 'frequencies';
        } else if (initialLocation.indexOf(this.staffTypesUrl) >= 0) {
            selectedItem = 'staffTypes';
        } else if (initialLocation.indexOf(this.usersUrl) >= 0) {
            selectedItem = 'users';
        } else {
            selectedItem = 'home';
        }

        return selectedItem;
    }
}

function mapStateToProps(state: ReduxStoreModel) {
    return {
        User: state.User,
    };
}

export default connect(mapStateToProps)(SideNav);
