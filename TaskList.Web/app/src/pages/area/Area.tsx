import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import AreaTable from './AreaTable';
import AreaEdit from './AreaEdit';
import Routes from 'config/ConfigureRoutes';
import PageNotFound from 'pages/public/PageNotFound';
import SubAreaEdit from 'pages/subArea/SubAreaEdit';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';

class Area extends React.Component<{}> {
    render() {
        return (
            <React.Fragment>
                <BreadcrumbsItem name="areas" to={Routes.GET.AREA_BASE}>Areas</BreadcrumbsItem>

                <Switch>
                    <Route exact path={Routes.GET.AREA_BASE} component={AreaTable} />
                    <Route exact path={Routes.GET.SUBAREA_EDIT} component={SubAreaEdit} />
                    <Route path={Routes.GET.AREA_EDIT} component={AreaEdit} />

                    {/* This needs to be the last item */}
                    <Route component={PageNotFound} />
                </Switch>
            </React.Fragment>
        );
    }
}

export default Area;
