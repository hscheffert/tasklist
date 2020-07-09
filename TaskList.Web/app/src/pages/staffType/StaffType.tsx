import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import StaffTypeTable from './StaffTypeTable';
import StaffTypeEdit from './StaffTypeEdit';
import Routes from 'config/ConfigureRoutes';
import PageNotFound from 'pages/public/PageNotFound';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';

class StaffType extends React.Component<{}> {
    render() {
        return (
            <React.Fragment>
                <BreadcrumbsItem name="staffTypes" to={Routes.GET.STAFF_TYPE_BASE}>Staff Types</BreadcrumbsItem>

                <Switch>
                    <Route exact path={Routes.GET.STAFF_TYPE_BASE} component={StaffTypeTable} />
                    <Route path={Routes.GET.STAFF_TYPE_EDIT} component={StaffTypeEdit} />

                    {/* This needs to be the last item */}
                    <Route component={PageNotFound} />
                </Switch>
            </React.Fragment>
        );
    }
}

export default StaffType;
