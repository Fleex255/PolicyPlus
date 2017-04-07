# What special terms does Policy Plus use?

This document defines the terms used in Policy Plus documentation, UI, and code.

* *ADMX workspace*: A collection of ADMX and ADML files, not necessarily all in the same folder, from which policy objects are loaded.
* *Basic state*: The state of a policy (Enabled, Disabled, or Not Configured), not including the extra configuration if Enabled.
* *Display code*: The nonlocalized (ADMX-internal) name of an item, usually a policy.
* *Info code*: The nonlocalized code for a policy's description.
* *Local GPO*: A Group Policy Object that applies to the machine or to all users, stored in `\Windows\System32\GroupPolicy`.
* *Policy element*: A configurable option in a policy beyond the basic state, e.g. a numeric value.
* *Policy loader*: An object that prepares, manages, and cleans up policy sources.
* *Policy object*: Any ADMX-loaded object that has a unique ID: policy, category, product, or support definition.
* *Policy source*: Where the Registry-based policy data is actually stored, either a POL file or a Registry branch.
* *Policy state*: The configured state of a policy, its basic state plus the state of any element.
* *Preference*: A policy that affects a section of the Registry outside the Policies branches and will therefore not be undone if the setting changes to Not Configured.
* *Presentation code*: The ADMX-internal identifier of a policy's presentation, used to match policy elements to presentation elements.
* *Presentation element*: A user interface element that appears in the additional configuration section of a policy, usually corresponding to a policy element, e.g. a spinner for a numeric value.
* *Section*: The type of GPO to which a policy applies: user, computer, or both.
* *Semantic Policy (SPOL) file*: A script specifying the desired state of policies in terms of their basic state and element states.
* *Semantic Policy (SPOL) fragment*: A part of a SPOL file, without the header, that specifies the desired state of one policy.
* *Support definition*: A set of rules that can be attached to a policy to state what products support the policy.
* *Unique ID*: A fully-qualified identifier for a policy object, made up of its ADMX file's namespace and the object's internal ID.
* *User GPO*: A Group Policy Object that applies to one user, stored in `\Windows\System32\GroupPolicyUsers`.
* *User hive*: A Registry hive file specific to a user profile, usually named `ntuser.dat`.